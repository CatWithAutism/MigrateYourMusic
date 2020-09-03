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
    public class VKTrackListDownloader : ISongListDownloader<User>
    {
        public const int MAX_AUDIO_PER_REQUEST = 6000;
        private readonly VkApi _api;

        /// <summary>
        /// Конструктор подразумевает авторизацию.
        /// </summary>
        /// <param name="api">Авторизованное API VK.</param>
        public VKTrackListDownloader(VkApi api)
        {
            if (api == null)
                throw new ArgumentNullException(nameof(api), "API cannot be null.");
            else if (!api.IsAuthorized)
                throw new ArgumentException(nameof(api), "API has to be authorized.");

            _api = api;
        }

        /// <summary>
        /// Получает список треков пользователя по ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Track> GetTrackList(User user)
        {
            long tracksCount = _api.Audio.GetCount(user.Id);
            if (tracksCount <= 0)
            {
                return null;
            }

            if (tracksCount > MAX_AUDIO_PER_REQUEST)
            {
                List<Track> tracksList = new List<Track>();

                for (int currentOffset = 0; currentOffset < tracksCount; currentOffset += MAX_AUDIO_PER_REQUEST)
                {
                    var trackCollection = _api.Audio.Get(new AudioGetParams { OwnerId = user.Id, Offset = currentOffset });
                    tracksList.AddRange(trackCollection.Select(t => new Track { Artist = t.Artist, Title = t.Title, Duration = t.Duration }));
                }

                return tracksList;
            }
            else
            {
                var trackCollection = _api.Audio.Get(new AudioGetParams { OwnerId = user.Id });
                return trackCollection.Select(t => new Track { Artist = t.Artist, Title = t.Title, Duration = t.Duration });
            }
        }
    }
}
