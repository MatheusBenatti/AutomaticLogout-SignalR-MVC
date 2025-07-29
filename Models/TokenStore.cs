using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace websocket.Models
{
  public class TokenStore
  {
    // Token string -> username + status
    private static ConcurrentDictionary<string, bool> _validTokens = new ConcurrentDictionary<string, bool>();

    public static void AddToken(string token)
    {
      _validTokens[token] = true;
    }

    public static void InvalidateToken(string token)
    {
      if (string.IsNullOrWhiteSpace(token))
        return;

      _validTokens[token] = false;
    }

    public static bool IsTokenValid(string token)
    {
      return _validTokens.TryGetValue(token, out bool isValid) && isValid;
    }
  }
}