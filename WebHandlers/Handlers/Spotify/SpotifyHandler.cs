using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using SpotifyAPI.Web;

using WebHandlers.Interfaces;
using WebHandlers.Models;
using WebHandlers.Utils;

namespace WebHandlers.Handlers.Spotify
{
    public class SpotifyHandler : SpotifyClient, IWebHandler<SpotifyTrack>
    {
        public SpotifyHandler(IToken token) : base(token) { }

        public SpotifyHandler(SpotifyClientConfig config) : base(config) { }

        public SpotifyHandler(string token, string tokenType = "Bearer") : base(token, tokenType) { }

        /// <summary>
        /// Возвращает авторизованный клиент используя клиент и секрет ID.
        /// Аналог конструктора с использованием <see cref="SpotifyClientConfig"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="secretId"></param>
        /// <returns></returns>
        public static SpotifyHandler GetAuthorizedByIds(string clientId, string secretId)
        {
            Guarantee.IsStringNotNullOrEmpty(clientId, nameof(clientId));
            Guarantee.IsStringNotNullOrEmpty(secretId, nameof(secretId));

            return new SpotifyHandler(SpotifyClientConfig.CreateDefault()
                .WithAuthenticator(new ClientCredentialsAuthenticator(clientId, secretId)));
        }

        /// <summary>
        /// Search the similar track on spotify via default track model.
        /// </summary>
        /// <param name="track">Track model</param>
        /// <returns>Spotify track info.</returns>
        public SpotifyTrack FindTrackPair(Track track)
            => FindTrackPairAsync(track, new CancellationToken()).GetAwaiter().GetResult();


        /// <summary>
        /// Async search the similar track on spotify via default track model.
        /// </summary>
        /// <param name="track">Track model</param>
        /// <param name="ct"></param>
        /// <returns>Spotify track info.</returns>
        public async Task<SpotifyTrack> FindTrackPairAsync(Track track, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            
            Guarantee.IsArgumentNotNull(track, nameof(track));

            SearchResponse taskResult = await Search.Item(
                new SearchRequest(SearchRequest.Types.Track, $"{track.Artist} {track.Title}")
                {
                    Limit = 1
                });

            if (taskResult.Tracks.Items != null && taskResult.Tracks.Items.Any())
            {
                //Extract all useful information
                return (SpotifyTrack)taskResult.Tracks.Items.FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// Search similar tracks on spotify via default track model.
        /// </summary>
        /// <param name="tracks">Tracks models</param>
        /// <param name="delay"></param>
        /// <param name="progress"></param>
        /// <returns>Dictionary of default models and spotify tracks.</returns>
        public Dictionary<Track, SpotifyTrack> FindTracksPairs(IEnumerable<Track> tracks, int delay, Action<float> progress = null)
            => FindTracksPairsAsync(tracks, delay, new CancellationToken(), progress).GetAwaiter().GetResult();

        /// <summary>
        /// Async search similar tracks on spotify via default track model.
        /// </summary>
        /// <param name="tracks">Tracks models</param>
        /// <param name="delay"></param>
        /// <param name="ct"></param>
        /// <param name="progress"></param>
        /// <returns>Dictionary of default models and spotify tracks.</returns>
        public async Task<Dictionary<Track, SpotifyTrack>> FindTracksPairsAsync(IEnumerable<Track> tracks, int delay,
            CancellationToken ct, Action<float> progress = null)
        {
            Guarantee.IsEnumerableNotNullOrEmpty(tracks, nameof(tracks));
            Guarantee.IsGreaterThan(delay, nameof(delay), 0);

            Dictionary<Track, SpotifyTrack> songPairs = new Dictionary<Track, SpotifyTrack>();
            try
            {
                int processed = 0;
                int tracksCount = tracks.Count();

                foreach (var track in tracks)
                {
                    ct.ThrowIfCancellationRequested();
                    
                    SpotifyTrack spotifyTrack = await FindTrackPairAsync(track, new CancellationToken());
                    if (spotifyTrack != null)
                    {
                        songPairs.Add(track, spotifyTrack);
                    }

                    progress?.Invoke(100f / tracksCount * ++processed);
                    
                    await Task.Delay(delay, ct);
                }

                return songPairs;
            }
            catch
            {
                return songPairs;
            }
        }

        /// <summary>
        /// Save tracks to the user's library.
        /// </summary>
        /// <param name="spotifyTracks"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public void SaveTracks(IEnumerable<SpotifyTrack> spotifyTracks, int delay)
            => SaveTracksAsync(spotifyTracks, delay, new CancellationToken()).GetAwaiter().GetResult();

        /// <summary>
        /// Save tracks to the user's library.
        /// </summary>
        /// <param name="spotifyTracks"></param>
        /// <param name="delay"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task SaveTracksAsync(IEnumerable<SpotifyTrack> spotifyTracks, int delay, CancellationToken ct)
        {
            Guarantee.IsEnumerableNotNullOrEmpty(spotifyTracks, nameof(spotifyTracks));
            Guarantee.IsGreaterThan(delay, nameof(delay), 0);

            foreach(var list in CommonUtils.SplitList(spotifyTracks, 50))
            {
                await Library.SaveTracks(new LibrarySaveTracksRequest(list.Select(t => t.Id).ToList()));
                await Task.Delay(delay, ct);
                ct.ThrowIfCancellationRequested();
            }
        }
    }
}
