using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using websocket.Models;
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

    List<User> users = new List<User>
    {
      new User { Username = "admin", Password = "123" },
      new User { Username = "user", Password = "123" }
    };

    [HttpPost]
    public ActionResult Login(string username, string password)
    {
      bool exists = users.Any(u => u.Username == username && u.Password == password);

      if (exists)
      {
        Session["User"] = username;
        (_logoutService as LogoutService)?.SetLoggedIn(username);
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