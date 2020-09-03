using System;
using System.Collections.Generic;
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
        public Uri AlbumPicture { get; set; }

        /// <summary>
        /// Уникальный адресс внутри спотифая.
        /// </summary>
        public string SpotifyUri { get; set; }

    }
}
