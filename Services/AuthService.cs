using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Web;
using websocket.Models;

namespace websocket.Services
{
  public class AuthService : IAuthService
  {
    private static readonly ConcurrentDictionary<string, bool> _manualLogouts = new ConcurrentDictionary<string, bool>();

    private static readonly List<User> listUsers = new List<User>
    {
      new User { Username = "admin", Password = "123" },
      new User { Username = "user", Password = "123" }
    };
    public void LoginUser(string username, string password)
    {
      var user = listUsers.Find(u => u.Username == username && u.Password == password)
        ?? throw new UnauthorizedAccessException("Usuário ou senha inválidos");
    }

    // criado para conseguir deslogar pelo hub do signalr
    public void LogoutUser(string username, string token, bool manualLogout)
    {
      if (manualLogout) { 
        // caso seja logout intencional, nao deslogar novamente pelo signalr
        MarkManualLogout(username);
      }
      TokenStore.InvalidateToken(token);
    }

    public void CleanCookies(HttpContextBase httpContext)
    {
      if (httpContext == null)
      {
        throw new ArgumentNullException(nameof(httpContext));
      }

      var authManager = httpContext.GetOwinContext().Authentication;

      authManager.SignOut("ApplicationCookie");

      httpContext.Session?.Clear();
    }

    public static void MarkManualLogout(string username)
    {
      _manualLogouts[username] = true;
    }
    public bool WasManualLogout(string username)
    {
      return _manualLogouts.TryRemove(username, out _);
    }
    
  }
}