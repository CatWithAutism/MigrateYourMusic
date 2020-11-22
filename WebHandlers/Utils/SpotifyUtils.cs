using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebHandlers.Utils
{
    public static class SpotifyUtils
    {
        /// <summary>
        /// Generates URI to give permissions to your application.
        /// </summary>
        /// <param name="redirectUri"></param>
        /// <param name="clientId"></param>
        /// <param name="responseType"></param>
        /// <param name="scopes"></param>
        /// <returns></returns>
        public static string GetLoginUri(string redirectUri, string clientId, LoginRequest.ResponseType responseType, params string[] scopes)
            => new LoginRequest(new Uri(redirectUri), clientId, responseType) { Scope = scopes }.ToUri().ToString();
    }
}
