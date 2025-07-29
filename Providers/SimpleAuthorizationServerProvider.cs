using Microsoft.Owin.Security.OAuth;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using websocket.Models;
using websocket.Services;

namespace websocket.Providers
{
  public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
  {
    private readonly IAuthService _authService;
    public SimpleAuthorizationServerProvider(IAuthService authService)
    {
      _authService = authService;
    }
    
    public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
    {
      context.Validated();
    }
    public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
    {
      try
      {
        _authService.LoginUser(context.UserName, context.Password);

        var identity = new ClaimsIdentity(context.Options.AuthenticationType);
        identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));

        context.Validated(identity);
      }
      catch (UnauthorizedAccessException)
      {
        context.SetError("invalid_grant", "Usuário ou senha inválidos");
      }
    }

    public override Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context)
    {
      var token = context.AccessToken; // token gerado
      TokenStore.AddToken(token); // adiciona o token ao store (ativo)

      return base.TokenEndpointResponse(context);
    }
  }
}