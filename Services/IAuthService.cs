using System.Web;

namespace websocket.Services
{
  public interface IAuthService
  {
    void LoginUser(string username, string password);
    void LogoutUser(string username, string token, bool manualLogout);
    bool WasManualLogout(string username);
    void CleanCookies(HttpContextBase httpContext);
  }
}
