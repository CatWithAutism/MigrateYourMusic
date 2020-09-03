using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpotifyAPI.Web;

using WebHandlers.Models;

namespace WebHandlers.Handlers.Spotify
{
    public static class SpotifyHandler
    {
        private const string CLIENT_ID = "";
        private const string SECRET_ID = "";

        private static readonly SpotifyClient _spotifyClient;

        static SpotifyHandler()
        {
            SpotifyClientConfig config = SpotifyClientConfig
                .CreateDefault()
                .WithAuthenticator(new ClientCredentialsAuthenticator(CLIENT_ID, SECRET_ID));

            _spotifyClient = new SpotifyClient(config);
        }

        public static SpotifyTrack FindTrackPair(Track track)
            => FindTrackPairAsync(track).GetAwaiter().GetResult();

        public static async Task<SpotifyTrack> FindTrackPairAsync(Track track)
        {
            SearchResponse taskResult = await _spotifyClient.Search.Item(new SearchRequest(SearchRequest.Types.Track, $"{track.Artist} {track.Title}"));
            if (taskResult.Tracks.Items.Count > 0)
            {
                FullTrack spotifyTrack = taskResult.Tracks.Items.FirstOrDefault();

                //Вытаскиваем всю полезную информацию.
                return new SpotifyTrack()
                {
                    Artist = string.Join(" ", spotifyTrack.Artists.Select(t => t.Name)),
                    Album = spotifyTrack.Album.Name,
                    Title = spotifyTrack.Name,
                    Duration = spotifyTrack.DurationMs / 1000,
                    SpotifyUri = spotifyTrack.Uri,
                    AlbumPicture = new Uri(spotifyTrack.Album.Images.FirstOrDefault().Url)
                };
            }

            return null;
        }

        public static async Task<Dictionary<Track, SpotifyTrack>> FindTracksPairsAsync(IEnumerable<Track> tracks)
        {
            var songPairs = new Dictionary<Track, SpotifyTrack>();
            try
            {
                foreach (var track in tracks)
                {
                    SpotifyTrack spotifyTrack = await FindTrackPairAsync(track);
                    if (spotifyTrack != null)
                    {
                        songPairs.Add(track, spotifyTrack);
                    }
                }

                return songPairs;
            }
            catch
            {
                return songPairs;
            }
        }

        public static Dictionary<Track, SpotifyTrack> FindTracksPairs(IEnumerable<Track> tracks)
            => FindTracksPairsAsync(tracks).GetAwaiter().GetResult();
    }
}
