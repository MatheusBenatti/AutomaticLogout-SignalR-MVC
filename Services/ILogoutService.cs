namespace websocket.Services
{
  public interface ILogoutService
  {
    void LogoutUser(string username);
    bool IsLoggedIn(string username);
  }
}
