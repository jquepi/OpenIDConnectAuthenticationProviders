using System.Linq;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Infrastructure;

namespace Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Tokens
{
    public class StateChainer : IStateChainer
    {
        public string Chain(string callerState, string additionalState)
        {
            return $"{State.Encode(callerState)}.{State.Encode(additionalState)}";
        }

        public string[] Delink(string returnedState)
        {
            return !returnedState.Contains(".") ? new []{ State.Decode(returnedState) } : returnedState.Split('.').Select(State.Decode).ToArray();
        }
    }

    public interface IStateChainer
    {
        string Chain(string callerState, string additionalState);
        string[] Delink(string returnedState);
    }
}