﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using PoshBoutique.Providers;
using PoshBoutique.Identity;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Facebook;
using System.Security.Claims;
using PoshBoutique.Factories;
using Microsoft.Owin.Security.Google;
using System.Configuration;

namespace PoshBoutique
{
    public partial class Startup
    {
        static Startup()
        {
            PublicClientId = "self";

            UserManagerFactory = () => new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId, UserManagerFactory),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = false
            };
        }

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static Func<ApplicationUserManager> UserManagerFactory { get; set; }

        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "DE9rrDoJOJhbrGR9BBMpyBwa6",
            //    consumerSecret: "bsO1Wz3qx3kUsSIVk0s1ycIWsekDvR8P33m45CDjoU9Qi6YgY1");

            IFacebookAuthenticationFactory facebookAuthenticationFactory = new FacebookAuthenticationFactory();
            FacebookAuthenticationOptions facebookAuthenticationOptions = facebookAuthenticationFactory.CreateAuthenticationOptions();
            app.UseFacebookAuthentication(facebookAuthenticationOptions);

            GoogleOAuth2AuthenticationOptions googleOAuth2AuthenticationOptions = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = ConfigurationManager.AppSettings["oAuth2.Google.ClientId"],
                ClientSecret = ConfigurationManager.AppSettings["oAuth2.Google.ClientSecret"]
            };
            googleOAuth2AuthenticationOptions.Scope.Add("profile");
            googleOAuth2AuthenticationOptions.Scope.Add("email");

            app.UseGoogleAuthentication(googleOAuth2AuthenticationOptions);
        }
    }
}
