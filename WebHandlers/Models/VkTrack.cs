using System;
using VkNet.Model.Attachments;
using WebHandlers.Utils;

namespace WebHandlers.Models
{
    public class VkTrack : Track
    {
        /// <summary>
        /// Владелец трека.
        /// </summary>
        public long? OwnerId { get; set; }

        public static explicit operator VkTrack(Audio track)
        {
            Guarantee.IsArgumentNotNull(track, nameof(track));
            return new VkTrack
            {
                Artist = track.Artist,
                Title = track.Title,
                Duration = track.Duration,
                OwnerId = track.OwnerId
            };
        }
    }
}
