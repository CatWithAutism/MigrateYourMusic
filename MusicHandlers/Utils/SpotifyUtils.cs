using System;
using MusicHandlers.Models;
using MusicHandlers.SearchEngines.Spotify;
using SpotifyAPI.Web;

namespace MusicHandlers.Utils
{
    public static class SpotifyUtils
    {
        /// <summary>
        ///     Generates URI to give permissions to your application.
        /// </summary>
        /// <param name="redirectUri"></param>
        /// <param name="clientId"></param>
        /// <param name="responseType"></param>
        /// <param name="scopes"></param>
        /// <returns></returns>
        public static string GetLoginUri(string redirectUri, string clientId, LoginRequest.ResponseType responseType,
            params string[] scopes)
        {
            return new LoginRequest(new Uri(redirectUri), clientId, responseType) {Scope = scopes}.ToUri().ToString();
        }
        
        /// <summary>
        ///     Returns an authorized client via clientID and secretID.
        ///     This is an analog of constructor which is using <see cref="SpotifyClientConfig" />
        /// </summary>
        /// <param name="clientId">ClientID which you can get from your SpotifyApp</param>
        /// <param name="secretId">SecretID which you can get from your SpotifyApp</param>
        /// <returns>Instance of <see cref="SpotifySearchEngine{TIn}" /></returns>
        public static SpotifyClient GetAuthorizedByIds(string clientId, string secretId)
        {
            Guarantee.IsStringNotNullOrEmpty(clientId, nameof(clientId));
            Guarantee.IsStringNotNullOrEmpty(secretId, nameof(secretId));

            return new SpotifyClient(SpotifyClientConfig.CreateDefault()
                .WithAuthenticator(new ClientCredentialsAuthenticator(clientId, secretId)));
        }
    }
}