using System;
namespace PoshBoutique.Factories
{
    interface IFacebookAuthenticationFactory
    {
        Microsoft.Owin.Security.Facebook.FacebookAuthenticationOptions CreateAuthenticationOptions();
        Microsoft.Owin.Security.Facebook.IFacebookAuthenticationProvider CreateAuthenticationProvider(string authenticationType);
    }
}
