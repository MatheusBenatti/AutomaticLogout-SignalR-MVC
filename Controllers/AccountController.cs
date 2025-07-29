using Microsoft.Owin.Security;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using websocket.Models;
using websocket.Services;

namespace websocket.Controllers
{
  public class AccountController : Controller
  {

    private readonly IAuthService _authService;

    public AccountController() : this(new AuthService())
    {
    }
    public AccountController(IAuthService logoutService)
    {
      _authService = logoutService;
    }
    public ActionResult Login()
    {
      return View();
    }



    [HttpPost]
    public async Task<ActionResult> Login(string username, string password)
    {
      using (var client = new HttpClient())
      {
        var values = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("username", username),
            new KeyValuePair<string, string>("password", password)
        };

        var content = new FormUrlEncodedContent(values);
        var response = await client.PostAsync("https://localhost:44333/token", content);

        if (response.IsSuccessStatusCode)
        {
          var result = await response.Content.ReadAsAsync<TokenResponse>();

          // cookie de autenticação MVC
          var claims = new List<Claim>
          {
            new Claim(ClaimTypes.Name, username),
            new Claim("access_token", result.Access_token)
          };
          var identity = new ClaimsIdentity(claims, "ApplicationCookie");

          var ctx = Request.GetOwinContext();
          var authManager = ctx.Authentication;

          authManager.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);

          //passando para infos para o hub
          TempData["access_token"] = result.Access_token;
          TempData["username"] = username;

          return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Usuário ou senha inválidos";
        return View();
      }
    }
    public ActionResult Logout(string accessToken)
    {
      var identity = (ClaimsIdentity)User.Identity;
      var username = identity.Name;

      _authService.LogoutUser(username, accessToken, true);
      _authService.CleanCookies(this.HttpContext);

      return RedirectToAction("Login");
    }
  }

}