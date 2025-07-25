using System;
using System.Collections.Concurrent;
using System.Web;

namespace websocket.Services
{
  public class LogoutService : ILogoutService
  {
    private static ConcurrentDictionary<string, bool> _loggedUsers = new ConcurrentDictionary<string, bool>();
    public void SetLoggedIn(string username)
    {
      _loggedUsers[username] = true;
    }
    public void LogoutUser(string username)
    {
      _loggedUsers[username] = false;

      if (HttpContext.Current != null && HttpContext.Current.Session != null)
      {
        HttpContext.Current.Session.Clear();
        HttpContext.Current.Session.Abandon();
      }
      Console.WriteLine($"Usuário {username} deslogado pelo LogoutService.");
    }

    //verifica se user esta logado na primeira req
    public bool IsLoggedIn(string username)
    {
      return _loggedUsers.TryGetValue(username, out bool logged) && logged;
    }
  }
}