﻿using System;
using System.Collections.Generic;
using System.Linq;
using MusicCarriers.Engines.Interfaces;
using MusicCarriers.Models;
using MusicCarriers.Utils;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace MusicCarriers.Engines.DownloadEngines.VK
{
    public class VkMusicDownloadEngine : IMusicDownloadEngine<VkTrack, User>
    {
        private readonly VkApi _api;
        private readonly uint _maxAudioPerRequest;

        /// <summary>
        ///     This constructor assumes authorization.
        /// </summary>
        /// <param name="api">Authorized <see cref="VkApi" /></param>
        /// <param name="maxAudioPerRequest">Max count of tracks per one request. Should be less than 6k and more than 0</param>
        public VkMusicDownloadEngine(VkApi api, uint maxAudioPerRequest)
        {
            Guarantee.IsArgumentNotNull(api, nameof(api));
            Guarantee.IsGreaterThan(maxAudioPerRequest, 0, nameof(maxAudioPerRequest));
            Guarantee.IsLessOrEqual(maxAudioPerRequest, 6000, nameof(maxAudioPerRequest));

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