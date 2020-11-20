using System;
using System.Collections.Generic;
using System.Text;

using VkNet.Model.Attachments;

namespace WebHandlers.Models
{
    public class VkTrack : Track
    {
        /// <summary>
        /// Владелец трека.
        /// </summary>
        public long? OwnerId { get; set; }

        public static explicit operator VkTrack(Audio audio)
            => new VkTrack
            {
                Artist = audio.Artist,
                Title = audio.Title,
                Duration = audio.Duration,
                OwnerId = audio.OwnerId
            };
    }
}
