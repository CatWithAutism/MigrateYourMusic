using WebHandlers.Models;
using VkNet.AudioBypassService.Extensions;
using VkNet;
using VkNet.Model.RequestParams;
using VkNet.Exception;

using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using VkNet.Model;
using System.Linq;
using WebHandlers.Interfaces;
using WebHandlers.Utils;

namespace WebHandlers.Downloaders.VK
{
    public class VkTrackListDownloader : ITrackListDownloader<VkTrack, User>
    {
        private readonly uint _maxAudioPerRequest;
        private readonly VkApi _api;

        /// <summary>
        /// Конструктор подразумевает авторизацию.
        /// </summary>
        /// <param name="api">Авторизованное API VK.</param>
        /// <param name="maxAudioPerRequest">Максимальное количество треков за запрос. Не может быть больше 6к</param>
        public VkTrackListDownloader(VkApi api, uint maxAudioPerRequest)
        {
            Guarantee.IsArgumentNotNull(api, nameof(api));
            Guarantee.IsLessOrEqual(maxAudioPerRequest, nameof(maxAudioPerRequest), 0);
            Guarantee.IsGreaterThan(maxAudioPerRequest, nameof(maxAudioPerRequest), 6000);

            if (!api.IsAuthorized)
                throw new ArgumentException(nameof(api), "API has to be authorized.");

            _api = api;
            _maxAudioPerRequest = maxAudioPerRequest;
        }

        /// <summary>
        /// Получает список треков пользователя по ID.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IEnumerable<VkTrack> DownloadTrackList(User user)
        {
            Guarantee.IsArgumentNotNull(user, nameof(user));

            long tracksCount = _api.Audio.GetCount(user.Id);
            if (tracksCount <= 0)
                return null;

            if (tracksCount > _maxAudioPerRequest)
            {
                List<VkTrack> tracksList = new List<VkTrack>();

                for (uint songListOffset = 0; songListOffset < tracksCount; songListOffset += _maxAudioPerRequest)
                {
                    var vkAudioCollection = _api.Audio.Get(new AudioGetParams
                    {
                        OwnerId = user.Id,
                        Offset = songListOffset,
                        Count = _maxAudioPerRequest
                    });

                    tracksList.AddRange((IEnumerable<VkTrack>)vkAudioCollection);
                }

                return tracksList;
            }
            else
            {
                var vkAudioCollection = _api.Audio.Get(new AudioGetParams { OwnerId = user.Id });

                return (IEnumerable<VkTrack>)vkAudioCollection;
            }
        }
    }
}
