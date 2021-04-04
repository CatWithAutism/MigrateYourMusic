using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MusicCarriers.Engines.Interfaces;
using MusicCarriers.Models;
using MusicCarriers.Utils;
using SpotifyAPI.Web;

namespace MusicCarriers.Engines.SearchEngines.Spotify
{
    /// <summary>
    /// Wrapper of the <see cref="SpotifyClient"/>
    /// </summary>
    public class SpotifySearchEngine<TIn> : IMusicSearchEngine<SpotifyTrack, TIn> 
        where TIn : Track
    {
        private readonly SpotifyClient _client;
        
        public SpotifySearchEngine(SpotifyClient client)
        {
            Guarantee.IsArgumentNotNull(client, nameof(client));
            
            _client = client;
        }

        /// <summary>
        ///     Search the similar track on spotify via default track model.
        /// </summary>
        /// <param name="track">Track model</param>
        /// <returns>Spotify track info.</returns>
        public SpotifyTrack FindTrackPair(TIn track)
        {
            return FindTrackPairAsync(track, new CancellationToken()).GetAwaiter().GetResult();
        }
        
        /// <summary>
        ///     Async searching of the similar track on spotify via default track model.
        /// </summary>
        /// <param name="track">Track model</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Spotify track info.</returns>
        public async Task<SpotifyTrack> FindTrackPairAsync(TIn track, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            
            Guarantee.IsArgumentNotNull(track, nameof(track));
            
            var taskResult = await _client.Search.Item(
                new SearchRequest(SearchRequest.Types.Track, $"{track.Artist} {track.Title}")
                {
                    Limit = 1
                });

            if (taskResult.Tracks.Items != null && taskResult.Tracks.Items.Any())
                return (SpotifyTrack) taskResult.Tracks.Items.FirstOrDefault();

            return null;
        }

        /// <summary>
        ///     Searching of similar tracks on spotify via default track model.
        /// </summary>
        /// <param name="tracks">Tracks models</param>
        /// <param name="progress">Callback to inform about current progress.</param>
        /// <returns>Dictionary of default models and spotify tracks.</returns>
        public IEnumerable<SpotifyTrack> FindTracksPairs(IEnumerable<TIn> tracks, Action<float> progress = null)
        {
            return FindTracksPairsAsync(tracks, new CancellationToken(), progress).GetAwaiter().GetResult();
        }

        /// <summary>
        ///     Async searching of similar tracks on spotify via default track model.
        /// </summary>
        /// <param name="tracks">Tracks models</param>
        /// <param name="ct">Cancellation token.</param>
        /// <param name="progress">Callback to inform about current progress.</param>
        /// <returns>Dictionary of default models and spotify tracks.</returns>
        public async Task<IEnumerable<SpotifyTrack>> FindTracksPairsAsync(IEnumerable<TIn> tracks, 
            CancellationToken ct, Action<float> progress = null)
        {
            Guarantee.IsEnumerableNotNullOrEmpty(tracks, nameof(tracks));

            var songPairs = new List<SpotifyTrack>();
            try
            {
                //to avoid enumeration if it isn't necessary
                var tracksCount = progress == null ? 0 : tracks.Count();
                var processed = 0;

                foreach (var track in tracks)
                {
                    ct.ThrowIfCancellationRequested();

                    SpotifyTrack spotifyTrack;
                    try
                    {
                        spotifyTrack = await FindTrackPairAsync(track, new CancellationToken());
                    }
                    catch (APITooManyRequestsException tooManyRequestsException)
                    {
                        await Task.Delay(tooManyRequestsException.RetryAfter + TimeSpan.FromSeconds(1), ct);
                        spotifyTrack = await FindTrackPairAsync(track, new CancellationToken());
                    }
                    
                    if (spotifyTrack != null) 
                        songPairs.Add(spotifyTrack);
                    
                    progress?.Invoke(100f / tracksCount * ++processed);
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