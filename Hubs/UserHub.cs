using Microsoft.AspNet.SignalR;
using System;
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
    private static readonly Lazy<ILogoutService> _logoutService = new Lazy<ILogoutService>(() => new LogoutService());

    public override Task OnConnected()
    {
      var username = Context.QueryString["username"];
      if (!string.IsNullOrEmpty(username))
      {
        _userConnections.AddOrUpdate(username, 1, (key, count) => count + 1);
        Console.WriteLine($"[Connected] Usuário: {username}. Conexões ativas: {_userConnections[username]}");
      }

      return base.OnConnected();
    }

    public List<string> GetActiveConnections()
    {
      return _userConnections.Keys.ToList();
    }

    //usado somente para logout por tempo de inatividade
    public void ForceLogout()
    {
      var username = Context.QueryString["username"];
      if (!string.IsNullOrEmpty(username))
      {
        _logoutService.Value.LogoutUser(username);
      }
    }

    public override Task OnDisconnected(bool stopCalled)
    {
      var username = Context.QueryString["username"];
      if (!string.IsNullOrEmpty(username))
      {
        _userConnections.AddOrUpdate(username, 0, (key, count) => count - 1);

        if (_userConnections.TryGetValue(username, out int connectionCount) && connectionCount <= 0)
        {
          Task.Run(async () =>
          {
            await Task.Delay(5000); // 5 segundos de tolerância

            if (_userConnections.TryGetValue(username, out int finalCount) && finalCount <= 0)
            {
              _userConnections.TryRemove(username, out _);
              Console.WriteLine($"[Desconectado] Última aba fechada para {username}. Deslogando...");

              _logoutService.Value.LogoutUser(username);
            }
          });
        }
      }
      return base.OnDisconnected(stopCalled);
    }
  }
}