using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using VkNet;
using VkNet.AudioBypassService.Extensions;
using VkNet.Model;

namespace WebHandlers.Utils
{
    public static class VKUtils
    {
        /// <summary>
        /// Получает пользователя по его уникальной URL.
        /// Например vk.com/blablabla
        /// Необходимо для получения ID пользователя, так как все запросы у ВК проходят через ID.
        /// </summary>
        /// <param name="screenName">Наименование пользователя. Например vk.com/blablabla</param>
        /// <param name="vkApi">Авторизованный VK API.</param>
        /// <returns>Если пользователь не будет найден, то вернет NULL.</returns>
        public static User GetUserByScreenName(string screenName, VkApi vkApi)
        {
            Guarantee.IsStringNotNullOrEmpty(screenName, nameof(screenName));
            Guarantee.IsArgumentNotNull(vkApi, nameof(vkApi));

            if(!vkApi.IsAuthorized)
            {
                throw new ArgumentException(nameof(vkApi), "API has to be authorized.");
            }

            ReadOnlyCollection<User> users = vkApi.Users.Get(new string[] { screenName });
            return users.FirstOrDefault();
        }

        public static VkApi AuthorizeApi(string login, string password)
        {
            Guarantee.IsStringNotNullOrEmpty(login, nameof(login));
            Guarantee.IsStringNotNullOrEmpty(password, nameof(password));

            ServiceCollection services = new ServiceCollection();
            services.AddAudioBypass();
            VkApi api = new VkApi(services);

            api.Authorize(new ApiAuthParams
            {
                Login = login,
                Password = password
            });

            return api;
        }
    }
}
