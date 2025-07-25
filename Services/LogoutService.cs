using System;
using System.Collections.Concurrent;

namespace websocket.Services
{
  public class LogoutService : ILogoutService
  {
    public void LogoutUser(string username)
    {
      Console.WriteLine($"Usuário {username} deslogado pelo LogoutService.");
    }
  }
}