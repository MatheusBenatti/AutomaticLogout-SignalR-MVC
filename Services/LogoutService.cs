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
      // Atribui o username à sessão da requisição atual.
      // Esta sessão é única para cada usuário/guia.
      if (HttpContext.Current != null && HttpContext.Current.Session != null)
      {
        HttpContext.Current.Session["User"] = username;
      }
      _loggedUsers.AddOrUpdate(username, true, (key, existingValue) => true);
    }
    public void LogoutUser(string username)
    {
      _loggedUsers[username] = false;

      if (HttpContext.Current != null && HttpContext.Current.Session != null)
      {
        HttpContext.Current.Session.Clear();
        HttpContext.Current.Session.Abandon();
      }
    }

    //verifica se user esta logado na primeira req
    public bool IsLoggedIn(string username)
    {
      return _loggedUsers.TryGetValue(username, out bool isLogged) && isLogged;
    }
  }
}