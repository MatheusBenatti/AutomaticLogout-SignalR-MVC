using Microsoft.Owin;
using System.Threading.Tasks;
using websocket.Models;

namespace websocket.Middleware
{
  public class TokenValidationMiddleware : OwinMiddleware
  {
    public TokenValidationMiddleware(OwinMiddleware next) : base(next) { }
    public override async Task Invoke(IOwinContext context)
    {
      var token = context.Request.Headers.Get("Authorization")?.Replace("Bearer ", "");

      if (!string.IsNullOrEmpty(token) && !TokenStore.IsTokenValid(token))
      {
        context.Response.StatusCode = 401; // Unauthorized
        return;
      }

      await Next.Invoke(context);
    }
  }
}