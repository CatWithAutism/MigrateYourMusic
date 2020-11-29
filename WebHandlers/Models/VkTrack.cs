using VkNet.Model.Attachments;
using WebHandlers.Utils;

namespace WebHandlers.Models
{
    /// <summary>
    /// The model which is using in this application to handle tracks from VK.
    /// </summary>
    public class VkTrack : Track
    {
        /// <summary>
        ///     Owner of the track.
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