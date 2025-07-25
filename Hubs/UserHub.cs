using Microsoft.AspNet.SignalR;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using websocket.Services;

namespace websocket.Hubs
{
  public class UserHub : Hub
  {
    private static ConcurrentDictionary<string, int> _userConnections = new ConcurrentDictionary<string, int>();
    private readonly ILogoutService _logoutService = new LogoutService();

    public override Task OnConnected()
    {
      var username = Context.QueryString["username"];
      if (!string.IsNullOrEmpty(username))
      {
        _userConnections.AddOrUpdate(username, 1, (key, old) => old + 1);
      }

      return base.OnConnected();
    }

    public List<string> GetActiveConnections()
    {
      return _userConnections.Keys.ToList();
    }

    public void ForceLogout()
    {
      var username = Context.QueryString["username"];
      if (!string.IsNullOrEmpty(username))
      {
        _logoutService.LogoutUser(username);
      }
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

        _ = Task.Run(async () =>
        {
          await Task.Delay(5000); // tempo para reconexão (refresh)

          if (!_userConnections.TryGetValue(username, out int finalCount) || finalCount <= 0)
          {
            ForceLogout();
          }
        });
      }

      return base.OnDisconnected(stopCalled);
    }
  }
}