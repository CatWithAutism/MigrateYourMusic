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

namespace WebHandlers.Downloaders.VK
{
    public class VkTrackListDownloader : ITrackListDownloader<User>
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
            if (api == null)
                throw new ArgumentNullException(nameof(api), "API cannot be null.");
            else if (!api.IsAuthorized)
                throw new ArgumentException(nameof(api), "API has to be authorized.");
            else if (maxAudioPerRequest <= 0)
                throw new ArgumentException(nameof(api), "You have to specify max number of tracks for each request.");
            else if (maxAudioPerRequest > 6000)
                throw new ArgumentException(nameof(api), $"{nameof(maxAudioPerRequest)} cannot be more than 6000.");

            _api = api;
            _maxAudioPerRequest = maxAudioPerRequest;
        }

        /// <summary>
        /// Получает список треков пользователя по ID.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IEnumerable<Track> DownloadTrackList(User user)
        {
            long tracksCount = _api.Audio.GetCount(user.Id);
            if (tracksCount <= 0)
            {
                return null;
            }

            if (tracksCount > _maxAudioPerRequest)
            {
                List<Track> tracksList = new List<Track>();

                for (uint currentOffset = 0; currentOffset < tracksCount; currentOffset += _maxAudioPerRequest)
                {
                    var vkAudioCollection = _api.Audio.Get(new AudioGetParams 
                    { 
                        OwnerId = user.Id, 
                        Offset = currentOffset, 
                        Count = _maxAudioPerRequest
                    });

                    IEnumerable<Track> trackCollection = vkAudioCollection.Select(t => new Track
                    {
                        Artist = t.Artist,
                        Title = t.Title,
                        Duration = t.Duration
                    });

                    tracksList.AddRange(trackCollection);
                }

                return tracksList;
            }
            else
            {
                var vkAudioCollection = _api.Audio.Get(new AudioGetParams { OwnerId = user.Id });

                return vkAudioCollection.Select(t => new Track 
                { 
                    Artist = t.Artist, 
                    Title = t.Title, 
                    Duration = t.Duration 
                });
            }
        }
    }
}
