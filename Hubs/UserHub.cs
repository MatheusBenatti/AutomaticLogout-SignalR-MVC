using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using websocket.Services;

namespace websocket.Hubs
{
  public class UserHub : Hub
  {
    private static ConcurrentDictionary<string, int> _userConnections = new ConcurrentDictionary<string, int>();
    private static readonly Lazy<IAuthService> _authService = new Lazy<IAuthService>(() => new AuthService());

    public override Task OnConnected()
    {
      var username = Context.QueryString["username"];
      var token = Context.QueryString["access_token"];


      _userConnections.AddOrUpdate(username, 1, (key, count) => count + 1);
      Console.WriteLine($"[Connected] Usuário: {username}. Conexões ativas: {_userConnections[username]}");
      return base.OnConnected();
    }

    public List<string> GetActiveConnections()
    {
      return _userConnections.Keys.ToList();
    }

    //usado somente para logout por tempo de inatividade
    public void ForceLogout(string username, string token, HttpContextBase httpContext)
    {
      if (!string.IsNullOrEmpty(token))
      {
        _authService.Value.LogoutUser(username, token, false);
        _authService.Value.CleanCookies(httpContext);
      }
    }

    public override async Task OnDisconnected(bool stopCalled)
    {
      var username = Context.QueryString["username"];
      var token = Context.QueryString["access_token"];
      var httpContext = Context.Request.GetHttpContext();

      if (!string.IsNullOrEmpty(username))
      {
        _userConnections.AddOrUpdate(username, 0, (key, count) => Math.Max(count - 1, 0));

        if (_userConnections.TryGetValue(username, out int connectionCount) && connectionCount <= 0)
        {
          await Task.Delay(3000);  // 3 segundos de tolerância

          _userConnections.TryGetValue(username, out int finalCount);

          if (finalCount <= 0 && !_authService.Value.WasManualLogout(username))
          {
            _userConnections.TryRemove(username, out _);
            ForceLogout(username, token, httpContext);
          }
        }
        ;
      }
      await base.OnDisconnected(stopCalled);
    }
  }
}