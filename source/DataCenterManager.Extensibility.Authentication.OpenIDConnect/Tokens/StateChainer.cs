using System.Linq;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Infrastructure;

namespace Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Tokens
{
    public class StateChainer : IStateChainer
    {
        public string Chain(params string[] states)
        {
            return string.Join(".", states.Select(State.Encode));
        }

        public string[] Delink(string returnedState)
        {
            return !returnedState.Contains(".") ? new []{ State.Decode(returnedState) } : returnedState.Split('.').Select(State.Decode).ToArray();
        }
    }

    public interface IStateChainer
    {
        string Chain(params string[] states);
        string[] Delink(string returnedState);
    }
}