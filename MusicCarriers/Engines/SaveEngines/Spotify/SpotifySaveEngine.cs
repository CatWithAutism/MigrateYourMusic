using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MusicCarriers.Engines.Interfaces;
using MusicCarriers.Models;
using MusicCarriers.Utils;
using SpotifyAPI.Web;

namespace MusicCarriers.Engines.SaveEngines.Spotify
{
    public class SpotifySaveEngine : IMusicSaveEngine<SpotifyTrack>
    {
        
        private readonly SpotifyClient _client;
        
        public SpotifySaveEngine(SpotifyClient client)
        {
            Guarantee.IsArgumentNotNull(client, nameof(client));

            _client = client;
        }
        
        /// <summary>
        ///     Save tracks into the library of user.
        /// </summary>
        /// <param name="spotifyTracks">Spotify tracks which is gonna be added.</param>
        /// <param name="delay">Delay between requests. Should be more than -1</param>
        /// <returns></returns>
        public bool SaveTracks(IEnumerable<SpotifyTrack> spotifyTracks, int delay)
            => SaveTracksAsync(spotifyTracks, new CancellationToken()).GetAwaiter().GetResult();

        /// <summary>
        ///     Async saving of tracks into the library of user.
        /// </summary>
        /// <param name="spotifyTracks">Spotify tracks which is gonna be added.</param>
        /// <param name="ct">Token to cancel.</param>
        /// <returns></returns>
        public async Task<bool> SaveTracksAsync(IEnumerable<SpotifyTrack> spotifyTracks, CancellationToken ct)
        {
            Guarantee.IsEnumerableNotNullOrEmpty(spotifyTracks, nameof(spotifyTracks));

            foreach (var list in CommonUtils.SplitOnLists(spotifyTracks, 50))
            {
                ct.ThrowIfCancellationRequested();
                
                try
                {
                    return await SaveTracks(list);
                }
                catch (APITooManyRequestsException tooManyRequestsException)
                {
                    await Task.Delay(tooManyRequestsException.RetryAfter + TimeSpan.FromSeconds(1), ct);

                    return await SaveTracks(list);
                }
            }

            return true;
        }

        private Task<bool> SaveTracks(IEnumerable<SpotifyTrack> enumerable)
        {
            return _client.Library.SaveTracks(new LibrarySaveTracksRequest(enumerable.Where(t => t != null)
                .Select(t => t.Id)
                .Distinct()
                .ToList()));
        }
    }
}