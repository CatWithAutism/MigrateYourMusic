using WebHandlers.Models;
using VkNet;
using VkNet.Model.RequestParams;
using System;
using System.Collections.Generic;
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
            Guarantee.IsGreaterThan(maxAudioPerRequest, nameof(maxAudioPerRequest), 0);
            Guarantee.IsLessOrEqual(maxAudioPerRequest, nameof(maxAudioPerRequest), 6000);

            if (!api.IsAuthorized)
                throw new ArgumentException("API has to be authorized.", nameof(api));

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

                    tracksList.AddRange(vkAudioCollection.Select(t => (VkTrack)t));
                }

                return tracksList;
            }
            else
            {
                var vkAudioCollection = _api.Audio.Get(new AudioGetParams { OwnerId = user.Id, Count = tracksCount});

                return vkAudioCollection.Select(t => (VkTrack)t);
            }
        }
    }
}
