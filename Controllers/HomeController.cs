using System.Security.Claims;
using System.Web.Mvc;
using websocket.Models;

namespace websocket.Controllers
{

  public class HomeController : Controller
  {
    [Authorize]
    public ActionResult Index()
    {
      var claimsIdentity = (ClaimsIdentity)User.Identity;
      var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
      var token = claimsIdentity.FindFirst("access_token").Value;
      ViewBag.Username = username;
      ViewBag.AccessToken = token;

      if (string.IsNullOrEmpty(token) || !TokenStore.IsTokenValid(token))
      {
        return RedirectToAction("Login", "Account");
      };

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