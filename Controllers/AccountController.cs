using System.Web.Mvc;
using websocket.Services;

namespace websocket.Controllers
{
  public class AccountController : Controller
  {

    private readonly ILogoutService _logoutService;

    public AccountController() : this(new LogoutService())
    {
    }
    public AccountController(ILogoutService logoutService)
    {
      _logoutService = logoutService;
    }
    public ActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Login(string username, string password)
    {
      if (username == "admin" && password == "123")
      {
        Session["User"] = username;
        return RedirectToAction("Index", "Home");
      }

      ViewBag.Error = "Usuário ou senha inválidos";
      return View();
    }

    public ActionResult Logout(string user = null)
    {
      var username = user ?? Session["User"]?.ToString();
      if (!string.IsNullOrEmpty(username))
      {
        _logoutService.LogoutUser(username);
      }

      Session.Clear();
      return RedirectToAction("Login");
    }
  }

}