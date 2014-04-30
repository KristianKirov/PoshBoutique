using Microsoft.Owin.Security.Facebook;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace PoshBoutique.Factories
{
    public class FacebookAuthenticationFactory : PoshBoutique.Factories.IFacebookAuthenticationFactory
    {
        private string TryGetValue(JObject user, string propertyName)
        {
            JToken jToken;
            if (!user.TryGetValue(propertyName, out jToken))
            {
                return null;
            }

            return jToken.ToString();
        }

        public IFacebookAuthenticationProvider CreateAuthenticationProvider(string authenticationType)
        {
            return new FacebookAuthenticationProvider()
            {
                OnAuthenticated = context =>
                {
                    context.Identity.AddClaim(new Claim(ClaimTypes.Email, context.Email, ClaimValueTypes.String, authenticationType));

                    string firstName = this.TryGetValue(context.User, "first_name");
                    context.Identity.AddClaim(new Claim(ClaimTypes.GivenName, firstName, ClaimValueTypes.String, authenticationType));

                    string lastName = this.TryGetValue(context.User, "last_name");
                    context.Identity.AddClaim(new Claim(ClaimTypes.Surname, lastName, ClaimValueTypes.String, authenticationType));

                    return Task.FromResult<object>(null);
                }
            };
        }

        public FacebookAuthenticationOptions CreateAuthenticationOptions()
        {
            FacebookAuthenticationOptions facebookAuthenticationOptions = new FacebookAuthenticationOptions()
            {
                AppId = "717955831558103",
                AppSecret = "b4c52444cce98a1f7bd03cbd55654194"
            };
            facebookAuthenticationOptions.Scope.Add("email");
            facebookAuthenticationOptions.Provider = this.CreateAuthenticationProvider(facebookAuthenticationOptions.AuthenticationType);

            return facebookAuthenticationOptions;
        }
    }
}