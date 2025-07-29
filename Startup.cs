using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using websocket.Middleware;
using websocket.Providers;
using websocket.Services;

[assembly: OwinStartup(typeof(websocket.Startup))]

namespace websocket
{
  public class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      // Oauth para autenticação via token para API'S
      var oauthOptions = new OAuthAuthorizationServerOptions
      {
        AllowInsecureHttp = true,
        TokenEndpointPath = new PathString("/token"),
        AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
        Provider = new SimpleAuthorizationServerProvider(new AuthService())
      };

      // Middleware de autenticação via cookie, para proteger views (MVC + [Authorize])
      app.UseCookieAuthentication(new CookieAuthenticationOptions
      {
        AuthenticationType = "ApplicationCookie",
        LoginPath = new PathString("/Account/Login"),
        ExpireTimeSpan = TimeSpan.FromMinutes(30),
        SlidingExpiration = true,
      });

      app.Use<TokenValidationMiddleware>();
      app.UseOAuthAuthorizationServer(oauthOptions);
      app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
      app.MapSignalR();
    }
  }
}