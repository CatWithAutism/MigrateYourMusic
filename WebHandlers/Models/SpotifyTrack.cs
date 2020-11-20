using SpotifyAPI.Web;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebHandlers.Models
{
    public class SpotifyTrack : Track
    {
        /// <summary>
        /// Альбом.
        /// </summary>
        public string Album { get; set; }

        /// <summary>
        /// Картинка от альбома.
        /// </summary>
        public string AlbumPictureUri { get; set; }

        /// <summary>
        /// Уникальный адресс внутри спотифая.
        /// </summary>
        public string SpotifyUri { get; set; }

        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public string Id { get; set; }

        public static explicit operator SpotifyTrack(FullTrack track)
            => new SpotifyTrack
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
