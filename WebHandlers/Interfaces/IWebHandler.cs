using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using WebHandlers.Models;

namespace WebHandlers.Interfaces
{
    public interface IWebHandler<T> where T : class
    {
        T FindTrackPair(Track track);

        Task<T> FindTrackPairAsync(Track track);

        Dictionary<Track, T> FindTracksPairs(IEnumerable<Track> tracks, int delay, CancellationToken ct, Action<float> progress = null);

        Task<Dictionary<Track, T>> FindTracksPairsAsync(IEnumerable<Track> tracks, int delay, CancellationToken ct, Action<float> progress = null);

        void SaveTracks(IEnumerable<T> spotifyTracks, int delay);

        Task SaveTracksAsync(IEnumerable<T> spotifyTracks, int delay);
    }
}