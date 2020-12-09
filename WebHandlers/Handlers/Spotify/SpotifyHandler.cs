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
    /// <summary>
    /// Wrapper of the <see cref="SpotifyClient"/>
    /// Implements: <see cref="IWebHandler{SpotifyHandler}"/>
    /// </summary>
    public class SpotifyHandler : SpotifyClient, IWebHandler<SpotifyTrack, Track>
    {
        public SpotifyHandler(IToken token) : base(token)
        {
        }

        public SpotifyHandler(SpotifyClientConfig config) : base(config)
        {
        }

        public SpotifyHandler(string token, string tokenType = "Bearer") : base(token, tokenType)
        {
        }

        /// <summary>
        ///     Search the similar track on spotify via default track model.
        /// </summary>
        /// <param name="track">Track model</param>
        /// <returns>Spotify track info.</returns>
        public SpotifyTrack FindTrackPair(Track track)
        {
            return FindTrackPairAsync(track, new CancellationToken()).GetAwaiter().GetResult();
        }

        /// <summary>
        ///     Async searching of the similar track on spotify via default track model.
        /// </summary>
        /// <param name="track">Track model</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Spotify track info.</returns>
        public async Task<SpotifyTrack> FindTrackPairAsync(Track track, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            Guarantee.IsArgumentNotNull(track, nameof(track));

            var taskResult = await Search.Item(
                new SearchRequest(SearchRequest.Types.Track, $"{track.Artist} {track.Title}")
                {
                    Limit = 1
                });

            if (taskResult.Tracks.Items != null && taskResult.Tracks.Items.Any())
                //Extract all useful information
                return (SpotifyTrack) taskResult.Tracks.Items.FirstOrDefault();

            return null;
        }

        /// <summary>
        ///     Searching of similar tracks on spotify via default track model.
        /// </summary>
        /// <param name="tracks">Tracks models</param>
        /// <param name="delay">Delay between requests. Should be more than -1</param>
        /// <param name="progress">Callback to inform about current progress.</param>
        /// <returns>Dictionary of default models and spotify tracks.</returns>
        public Dictionary<Track, SpotifyTrack> FindTracksPairs(IEnumerable<Track> tracks, int delay,
            Action<float> progress = null)
        {
            return FindTracksPairsAsync(tracks, delay, new CancellationToken(), progress).GetAwaiter().GetResult();
        }

        /// <summary>
        ///     Async searching of similar tracks on spotify via default track model.
        /// </summary>
        /// <param name="tracks">Tracks models</param>
        /// <param name="delay">Delay between requests. Should be more than -1</param>
        /// <param name="ct">Cancellation token.</param>
        /// <param name="progress">Callback to inform about current progress.</param>
        /// <returns>Dictionary of default models and spotify tracks.</returns>
        public async Task<Dictionary<Track, SpotifyTrack>> FindTracksPairsAsync(IEnumerable<Track> tracks, int delay,
            CancellationToken ct, Action<float> progress = null)
        {
            Guarantee.IsEnumerableNotNullOrEmpty(tracks, nameof(tracks));
            Guarantee.IsGreaterThan(delay, nameof(delay), 0);

            var songPairs = new Dictionary<Track, SpotifyTrack>();
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
                        //if happened then multiply delay by 2
                        delay *= 2;
                        await Task.Delay(tooManyRequestsException.RetryAfter.Milliseconds, ct);
                        spotifyTrack = await FindTrackPairAsync(track, new CancellationToken());
                    }
                    
                    if (spotifyTrack != null) songPairs.Add(track, spotifyTrack);
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
        ///     Save tracks into the library of user.
        /// </summary>
        /// <param name="spotifyTracks">Spotify tracks which is gonna be added.</param>
        /// <param name="delay">Delay between requests. Should be more than -1</param>
        /// <returns></returns>
        public bool SaveTracks(IEnumerable<SpotifyTrack> spotifyTracks, int delay)
        {
            return SaveTracksAsync(spotifyTracks, delay, new CancellationToken()).GetAwaiter().GetResult();
        }

        /// <summary>
        ///     Async saving of tracks into the library of user.
        /// </summary>
        /// <param name="spotifyTracks">Spotify tracks which is gonna be added.</param>
        /// <param name="delay">Delay between requests. Should be more than -1</param>
        /// <param name="ct">Token to cancel.</param>
        /// <returns></returns>
        public async Task<bool> SaveTracksAsync(IEnumerable<SpotifyTrack> spotifyTracks, int delay, CancellationToken ct)
        {
            Guarantee.IsEnumerableNotNullOrEmpty(spotifyTracks, nameof(spotifyTracks));
            Guarantee.IsGreaterOrEqual(delay, nameof(delay), 0);

            foreach (var list in CommonUtils.SplitOnLists(spotifyTracks, 50))
            {
                ct.ThrowIfCancellationRequested();
                
                try
                {
                    await Library.SaveTracks(new LibrarySaveTracksRequest(list.Where(t => t != null)
                        .Select(t => t.Id)
                        .Distinct()
                        .ToList()));
                }
                catch (APITooManyRequestsException tooManyRequestsException)
                {
                    //if happened then multiply delay by 2
                    delay *= 2;
                    //wait retryafter and go on
                    await Task.Delay(tooManyRequestsException.RetryAfter.Milliseconds, ct);
                    await Library.SaveTracks(new LibrarySaveTracksRequest(list.Where(t => t != null)
                        .Select(t => t.Id)
                        .Distinct()
                        .ToList()));
                }
                
                await Task.Delay(delay, ct);
            }

            return true;
        }

        /// <summary>
        ///     Returns an authorized client via clientID and secretID.
        ///     This is an analog of constructor which is using <see cref="SpotifyClientConfig" />
        /// </summary>
        /// <param name="clientId">ClientID which you can get from your SpotifyApp</param>
        /// <param name="secretId">SecretID which you can get from your SpotifyApp</param>
        /// <returns>Instance of <see cref="SpotifyHandler" /></returns>
        public static SpotifyHandler GetAuthorizedByIds(string clientId, string secretId)
        {
            Guarantee.IsStringNotNullOrEmpty(clientId, nameof(clientId));
            Guarantee.IsStringNotNullOrEmpty(secretId, nameof(secretId));

            return new SpotifyHandler(SpotifyClientConfig.CreateDefault()
                .WithAuthenticator(new ClientCredentialsAuthenticator(clientId, secretId)));
        }
    }
}