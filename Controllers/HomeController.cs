using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using websocket.Services;

namespace websocket.Controllers
{
  public class HomeController : Controller
  {
    private readonly ILogoutService _logoutService;

    public HomeController() : this(new LogoutService())
    {
    }

    public HomeController(ILogoutService logoutService)
    {
      _logoutService = logoutService;
    }

    public ActionResult Index()
    {
      var user = Session["User"] as string;
      if (string.IsNullOrEmpty(user) || !_logoutService.IsLoggedIn(user))
        return RedirectToAction("Login", "Account");

      return View();
    }

    public ActionResult About()
    {
      ViewBag.Message = "Your application description page.";

      return View();
    }

    public ActionResult Contact()
    {
      ViewBag.Message = "Your contact page.";

      return View();
    }
  }
}