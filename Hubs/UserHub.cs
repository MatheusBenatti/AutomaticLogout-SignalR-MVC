using Microsoft.AspNet.SignalR;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace websocket.Hubs
{
    public class UserHub : Hub
    {
        private static ConcurrentDictionary<string, int> _userConnections = new ConcurrentDictionary<string, int>();

        public override Task OnConnected()
        {
            var username = Context.QueryString["username"];
            if (!string.IsNullOrEmpty(username))
            {
                _userConnections.AddOrUpdate(username, 1, (key, old) => old + 1);
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var username = Context.QueryString["username"];
            if (!string.IsNullOrEmpty(username) && _userConnections.ContainsKey(username))
            {
                _userConnections.AddOrUpdate(username, 0, (key, old) => old - 1);

                if (_userConnections[username] <= 0)
                {
                    _userConnections.TryRemove(username, out _);
                }
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}