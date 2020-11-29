using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using VkNet;
using VkNet.AudioBypassService.Extensions;
using VkNet.Model;

namespace WebHandlers.Utils
{
    public static class VkUtils
    {
        /// <summary>
        ///     Gets <see cref="User" /> via his screen name.
        /// </summary>
        /// <param name="screenName">Screen name of the user. Example: vk.com/@ThisIsYourScreenName@</param>
        /// <param name="vkApi">Authorized Vk Api</param>
        /// <returns>If user is not found it returns NULL.</returns>
        /// <example><see cref="screenName" />="id345234523"</example>
        public static User GetUserByScreenName(string screenName, VkApi vkApi)
        {
            Guarantee.IsStringNotNullOrEmpty(screenName, nameof(screenName));
            Guarantee.IsArgumentNotNull(vkApi, nameof(vkApi));

            if (!vkApi.IsAuthorized)
                throw new ArgumentException("API has to be authorized.", nameof(vkApi));

            return vkApi.Users.Get(new[] {screenName})?.FirstOrDefault();
        }

        /// <summary>
        ///     Authorize <see cref="VkApi" /> by specified login and password.
        ///     Attaches VkAudioService to get music from VK.
        /// </summary>
        /// <param name="login">Your login.</param>
        /// <param name="password">Your password.</param>
        /// <returns>Authorized VK.</returns>
        public static VkApi AuthorizeApi(string login, string password)
        {
            Guarantee.IsStringNotNullOrEmpty(login, nameof(login));
            Guarantee.IsStringNotNullOrEmpty(password, nameof(password));

            var services = new ServiceCollection();
            services.AddAudioBypass();

            var api = new VkApi(services);
            api.Authorize(new ApiAuthParams
            {
                Login = login,
                Password = password
            });

            return api;
        }
    }
}