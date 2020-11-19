using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SpotifyAPI.Web;

using WebHandlers.Interfaces;
using WebHandlers.Models;
using WebHandlers.Utils;

namespace WebHandlers.SearchEngines.Spotify
{
    /// <summary>
    /// Осуществляет поисковые запросы по трекам на Spotify. Требует авторизации.
    /// </summary>
    public class SpotifySearchEngine : ITrackSearchEngine<SpotifyTrack>
    {
        private readonly SpotifyClient _spotifyClient;
        private readonly int _delay;

        /// <summary>
        /// Базовый конструктор.
        /// </summary>
        /// <param name="clientId">Клиентский ID из настроек приложения Spotify</param>
        /// <param name="secretId">Секретный ID из настроек приложения Spotify</param>
        /// <param name="delay">Ожидание между запросами.</param>
        public SpotifySearchEngine(string clientId, string secretId, int delay)
        {
            Guarantee.IsStringNotNullOrEmpty(clientId, nameof(clientId));
            Guarantee.IsStringNotNullOrEmpty(secretId, nameof(secretId));

            if (delay < 0)
                throw new ArgumentException(nameof(SpotifySearchEngine), $"{nameof(delay)} cannot be less than zero.");

            SpotifyClientConfig config = SpotifyClientConfig
                .CreateDefault()
                .WithAuthenticator(new ClientCredentialsAuthenticator(clientId, secretId));

            _spotifyClient = new SpotifyClient(config);
            _delay = delay;
        }

        /// <summary>
        /// Search the similar track on spotify via default track model.
        /// </summary>
        /// <param name="track">Track model</param>
        /// <returns>Spotify track info.</returns>
        public SpotifyTrack FindTrackPair(Track track)
            => FindTrackPairAsync(track).GetAwaiter().GetResult();


        /// <summary>
        /// Async search the similar track on spotify via default track model.
        /// </summary>
        /// <param name="track">Track model</param>
        /// <returns>Spotify track info.</returns>
        public async Task<SpotifyTrack> FindTrackPairAsync(Track track)
        {
            Guarantee.IsArgumentNotNull(track, nameof(track));

            SearchResponse taskResult = await _spotifyClient.Search.Item(
                new SearchRequest(SearchRequest.Types.Track, $"{track.Artist} {track.Title}"));

            if (taskResult.Tracks.Items.Count > 0)
            {
                FullTrack spotifyTrack = taskResult.Tracks.Items.FirstOrDefault();
                string imageLink = spotifyTrack.Album.Images.FirstOrDefault()?.Url;

                //Extract all useful information
                return new SpotifyTrack()
                {
                    Artist = string.Join(string.Empty, spotifyTrack.Artists.Select(t => t.Name)),
                    Album = spotifyTrack.Album.Name,
                    Title = spotifyTrack.Name,
                    Duration = spotifyTrack.DurationMs,
                    SpotifyUri = spotifyTrack.Uri,
                    AlbumPicture = string.IsNullOrWhiteSpace(imageLink) ? null : new Uri(imageLink)
                };
            }

            return null;
        }

        /// <summary>
        /// Search similar tracks on spotify via default track model.
        /// </summary>
        /// <param name="tracks">Tracks models</param>
        /// <returns>Dictionary of default models and spotify tracks.</returns>
        public Dictionary<Track, SpotifyTrack> FindTracksPairs(IEnumerable<Track> tracks)
            => FindTracksPairsAsync(tracks).GetAwaiter().GetResult();

        /// <summary>
        /// Async search similar tracks on spotify via default track model.
        /// </summary>
        /// <param name="tracks">Tracks models</param>
        /// <returns>Dictionary of default models and spotify tracks.</returns>
        public async Task<Dictionary<Track, SpotifyTrack>> FindTracksPairsAsync(IEnumerable<Track> tracks)
        {
            Guarantee.IsEnumerableNotNullOrEmpty(tracks, nameof(tracks));

            Dictionary<Track, SpotifyTrack> songPairs = new Dictionary<Track, SpotifyTrack>();
            try
            {
                foreach (var track in tracks)
                {
                    SpotifyTrack spotifyTrack = await FindTrackPairAsync(track);
                    if (spotifyTrack != null)
                    {
                        songPairs.Add(track, spotifyTrack);
                    }

                    await Task.Delay(_delay);
                }

                return songPairs;
            }
            catch
            {
                return songPairs;
            }
        }
    }
}
