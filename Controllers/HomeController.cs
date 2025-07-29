using System.Web.Mvc;
using websocket.Models;

namespace websocket.Controllers
{

  public class HomeController : Controller
  {
    [Authorize]
    public ActionResult Index()
    {
      //aplicando validacao por token enquando nao implemento redis
      var username = User.Identity.Name;
      var token = TempData["access_token"]?.ToString();

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