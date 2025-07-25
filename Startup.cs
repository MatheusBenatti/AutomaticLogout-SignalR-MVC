using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(websocket.Startup))]

namespace websocket
{
  public class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      app.MapSignalR();
    }
  }
}