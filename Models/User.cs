using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace websocket.Models
{
  public class User
  {
    internal static ClaimsIdentity Identity;

    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
  }

}