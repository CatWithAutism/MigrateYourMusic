using System.Linq;
using SpotifyAPI.Web;
using WebHandlers.Utils;

namespace WebHandlers.Models
{
    /// <summary>
    /// The model which is using in this application to handle tracks from Spotify.
    /// </summary>
    public class SpotifyTrack : Track
    {
        public string Album { get; set; }
        
        public string AlbumPictureUri { get; set; }
        
        public string SpotifyUri { get; set; }
        
        public string Id { get; set; }

        public static explicit operator SpotifyTrack(FullTrack track)
        {
            Guarantee.IsArgumentNotNull(track, nameof(track));
            return new SpotifyTrack
            {
                Id = track.Id,
                Artist = string.Join(string.Empty, track.Artists.Select(t => t.Name)),
                Album = track.Album.Name,
                Title = track.Name,
                Duration = track.DurationMs,
                SpotifyUri = track.Uri,
                AlbumPictureUri = track.Album.Images.FirstOrDefault()?.Url
            };
        }
    }
}