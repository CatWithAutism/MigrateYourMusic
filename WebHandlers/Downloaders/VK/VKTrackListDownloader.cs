using System;
using System.Collections.Generic;
using System.Linq;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;
using WebHandlers.Interfaces;
using WebHandlers.Models;
using WebHandlers.Utils;

namespace WebHandlers.Downloaders.VK
{
    public class VkTrackListDownloader : ITrackListDownloader<VkTrack, User>
    {
        private readonly VkApi _api;
        private readonly uint _maxAudioPerRequest;

        /// <summary>
        ///     This constructor assumes authorization.
        /// </summary>
        /// <param name="api">Authorized <see cref="VkApi" /></param>
        /// <param name="maxAudioPerRequest">Max count of tracks per one request. Should be less than 6k and more than 0</param>
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
        ///     Gets track list of specified user.
        /// </summary>
        /// <param name="user">User of Vk.</param>
        /// <returns>Collection of <see cref="VkTrack" /></returns>
        public IEnumerable<VkTrack> DownloadTrackList(User user)
        {
            Guarantee.IsArgumentNotNull(user, nameof(user));

            var tracksCount = _api.Audio.GetCount(user.Id);
            if (tracksCount <= 0)
                return null;

            if (tracksCount > _maxAudioPerRequest)
            {
                var tracksList = new List<VkTrack>();

                for (uint songListOffset = 0; songListOffset < tracksCount; songListOffset += _maxAudioPerRequest)
                {
                    var vkAudioCollection = _api.Audio.Get(new AudioGetParams
                    {
                        OwnerId = user.Id,
                        Offset = songListOffset,
                        Count = _maxAudioPerRequest
                    });

                    tracksList.AddRange(vkAudioCollection.Select(t => (VkTrack) t));
                }

                return tracksList;
            }
            else
            {
                var vkAudioCollection = _api.Audio.Get(new AudioGetParams
                {
                    OwnerId = user.Id,
                    Count = tracksCount
                });
                
                return vkAudioCollection.Select(t => (VkTrack) t);
            }
        }
    }
}