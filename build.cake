//////////////////////////////////////////////////////////////////////
// TOOLS
//////////////////////////////////////////////////////////////////////
#tool "nuget:?package=GitVersion.CommandLine&prerelease"

using Path = System.IO.Path;
using IO = System.IO;
using Cake.Common.Tools;

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////
var publishDir = "./publish";
var localPackagesDir = "../LocalPackages";
var artifactsDir = "./artifacts";
var assetDir = "./BuildAssets";

var extensionName = "Octopus.Server.Extensibility.Authentication.OpenIDConnect";
var extensionPath = "Server.Extensibility.Authentication.OpenIDConnect";

var bin451 = "/bin/Release/net451/";
var binNetStd = "/bin/Release/netstandard1.3/";

var extensionNodeName = "Octopus.Node.Extensibility.Authentication.OpenIDConnect";
var extensionNodePath = "Node.Extensibility.Authentication.OpenIDConnect";


var gitVersionInfo = GitVersion(new GitVersionSettings {
    OutputType = GitVersionOutput.Json
});

var nugetVersion = gitVersionInfo.NuGetVersion;

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////
Setup(context =>
{
    if(BuildSystem.IsRunningOnTeamCity)
        BuildSystem.TeamCity.SetBuildNumber(gitVersionInfo.NuGetVersion);
    if(BuildSystem.IsRunningOnAppVeyor)
        AppVeyor.UpdateBuildVersion(gitVersionInfo.NuGetVersion);

    Information("Building " + extensionName + " v{0}", nugetVersion);
});

Teardown(context =>
{
    Information("Finished running tasks.");
});

//////////////////////////////////////////////////////////////////////
//  PRIVATE TASKS
//////////////////////////////////////////////////////////////////////

Task("__Default")
    .IsDependentOn("__Clean")
    .IsDependentOn("__Restore")
    .IsDependentOn("__Build")
    .IsDependentOn("__Pack")
    .IsDependentOn("__Publish")
    .IsDependentOn("__CopyToLocalPackages");

Task("__Clean")
    .Does(() =>
{
    CleanDirectory(artifactsDir);
    CleanDirectory(publishDir);
    CleanDirectories("./source/**/bin");
    CleanDirectories("./source/**/obj");
});

Task("__Restore")
    .Does(() => DotNetCoreRestore("source"));

Task("__Build")
    .Does(() =>
{
    DotNetCoreBuild("./source", new DotNetCoreBuildSettings
    {
        Configuration = configuration,
        ArgumentCustomization = args => args.Append($"/p:Version={nugetVersion}")
    });
});

Task("__Pack")
    .Does(() => {
        var solutionDir = "./source/";
        
        // Server.Extensibility.Authentication.*
        
        var nugetPackDir = Path.Combine(publishDir, "nuget");
        var nuspecFile = extensionName + ".nuspec";
            
        CreateDirectory(nugetPackDir);
        CopyFileToDirectory(Path.Combine(assetDir, nuspecFile), nugetPackDir);

        Information(solutionDir + extensionName + bin451 + "System.IdentityModel.Tokens.Jwt.dll");
        CopyFileToDirectory(solutionDir + extensionPath + bin451 + "/System.IdentityModel.Tokens.Jwt.dll", nugetPackDir);
        CopyFileToDirectory(solutionDir + extensionPath + bin451 + extensionName + ".dll", nugetPackDir);
            
        CopyFileToDirectory(solutionDir + "Server.Extensibility.Authentication.AzureAD" + bin451 + "Octopus.Server.Extensibility.Authentication.AzureAD.dll", nugetPackDir);
        CopyFileToDirectory(solutionDir + "Server.Extensibility.Authentication.GoogleApps" + bin451 + "Octopus.Server.Extensibility.Authentication.GoogleApps.dll", nugetPackDir);

        NuGetPack(Path.Combine(nugetPackDir, nuspecFile), new NuGetPackSettings {
            Version = nugetVersion,
            OutputDirectory = artifactsDir
        });

        // Node.Extensibility.Authentication.*
        // DotNetCorePack("source/Node.Extensibility.Authentication.OpenIDConnect", new DotNetCorePackSettings
        
        var nugetNodePackDir = Path.Combine(publishDir, "nugetNode");
        var nugetNodePackDir451 = Path.Combine(nugetNodePackDir, "net451");
        var nugetNodePackDirStd = Path.Combine(nugetNodePackDir, "netstandard1.3");
        var nuspecNodeFile = extensionNodeName + ".nuspec";
            
        CreateDirectory(nugetNodePackDir);
        CreateDirectory(nugetNodePackDir451);
        CreateDirectory(nugetNodePackDirStd);
        CopyFileToDirectory(Path.Combine(assetDir, nuspecNodeFile), nugetNodePackDir);
            
        //Information(solutionDir + extensionName + bin451 + "System.IdentityModel.Tokens.Jwt.dll");
        //CopyFileToDirectory(solutionDir + extensionNodePath + bin451 + "System.IdentityModel.Tokens.Jwt.dll", nugetNodePackDir451);
        CopyFileToDirectory(solutionDir + extensionNodePath + bin451 + extensionNodeName + ".dll", nugetNodePackDir451);
		
        //CopyFileToDirectory(solutionDir + extensionNodePath + bin451 + "System.IdentityModel.Tokens.Jwt.dll", nugetNodePackDirStd);
		CopyFileToDirectory(solutionDir + extensionNodePath + binNetStd + extensionNodeName + ".dll", nugetNodePackDirStd);
            
        //CopyFileToDirectory(solutionDir + "Node.Extensibility.Authentication.AzureAD" + bin451 + "Octopus.Node.Extensibility.Authentication.AzureAD.dll", nugetNodePackDir);
        //CopyFileToDirectory(solutionDir + "Node.Extensibility.Authentication.GoogleApps" + bin451 + "Octopus.Node.Extensibility.Authentication.GoogleApps.dll", nugetNodePackDir);

        NuGetPack(Path.Combine(nugetNodePackDir, nuspecNodeFile), new NuGetPackSettings {
            Version = nugetVersion,
            OutputDirectory = artifactsDir
        });
    });


Task("__Publish")
    .WithCriteria(BuildSystem.IsRunningOnTeamCity)
    .Does(() =>
{
    NuGetPush($"{artifactsDir}/{extensionName}.{nugetVersion}.nupkg", new NuGetPushSettings {
        Source = "https://octopus.myget.org/F/octopus-dependencies/api/v3/index.json",
        ApiKey = EnvironmentVariable("MyGetApiKey")
    });
    
    if (gitVersionInfo.PreReleaseLabel == "")
    {
        NuGetPush($"{artifactsDir}/{extensionName}.{nugetVersion}.nupkg", new NuGetPushSettings {
            Source = "https://www.nuget.org/api/v2/package",
            ApiKey = EnvironmentVariable("NuGetApiKey")
        });
    }
});


Task("__CopyToLocalPackages")
    .WithCriteria(BuildSystem.IsLocalBuild)
    .IsDependentOn("__Pack")
    .Does(() =>
{
    CreateDirectory(localPackagesDir);
    CopyFileToDirectory(Path.Combine(artifactsDir, $"{extensionName}.{nugetVersion}.nupkg"), localPackagesDir);
});

private class AutoRestoreFile : IDisposable
{
    private byte[] _contents;
    private string _filename;
    public AutoRestoreFile(string filename)
    {
        _filename = filename;
        _contents = IO.File.ReadAllBytes(filename);
    }

    public void Dispose() => IO.File.WriteAllBytes(_filename, _contents);
}

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////
Task("Default")
    .IsDependentOn("__Default");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////
RunTarget(target);
