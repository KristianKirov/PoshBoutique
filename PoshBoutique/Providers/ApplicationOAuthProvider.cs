using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using PoshBoutique.Identity;
using System.Web;
using System.Collections.Specialized;

namespace PoshBoutique.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        private readonly Func<ApplicationUserManager> _userManagerFactory;

        public ApplicationOAuthProvider(string publicClientId, Func<ApplicationUserManager> userManagerFactory)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            if (userManagerFactory == null)
            {
                throw new ArgumentNullException("userManagerFactory");
            }

            _publicClientId = publicClientId;
            _userManagerFactory = userManagerFactory;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (ApplicationUserManager userManager = _userManagerFactory())
            {
                ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                ClaimsIdentity oAuthIdentity = await userManager.CreateIdentityAsync(user,
                    context.Options.AuthenticationType);
                ClaimsIdentity cookiesIdentity = await userManager.CreateIdentityAsync(user,
                    CookieAuthenticationDefaults.AuthenticationType);
                //AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user);
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, null);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri requestUri = context.Request.Uri;
                Uri redirectUri = new Uri(context.RedirectUri);
                if (redirectUri.Host == requestUri.Host && "/".Equals(redirectUri.AbsolutePath, StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.IsNullOrEmpty(redirectUri.Query))
                    {
                        NameValueCollection queryString = HttpUtility.ParseQueryString(redirectUri.Query);
                        string initialUrl = queryString["ru"];
                        if (!string.IsNullOrEmpty(initialUrl))
                        {
                            Uri initialUri = new Uri(initialUrl);
                            if (initialUri.Host == requestUri.Host)
                            {
                                context.Validated();
                            }
                        }
                    }
                }
            }

            return Task.FromResult<object>(null);
        }

        //public static AuthenticationProperties CreateProperties(ApplicationUser user)
        //{
        //    IDictionary<string, string> data = new Dictionary<string, string>
        //    {
        //        { "email", user.UserName },
        //        { "firstName", user.FirstName },
        //        { "lastName", user.LastName }
        //    };

        //    return new AuthenticationProperties(data);
        //}
    }
}