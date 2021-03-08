using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebHandlers.Models;

namespace WebHandlers.Interfaces
{
    public interface IWebHandler<T, V> 
        where T : class 
        where V : Track
    {
        T FindTrackPair(V track);

        Task<T> FindTrackPairAsync(V track, CancellationToken ct);

        Dictionary<V, T> FindTracksPairs(IEnumerable<V> tracks, Action<float> progress = null);

        Task<Dictionary<V, T>> FindTracksPairsAsync(IEnumerable<V> tracks, CancellationToken ct,
            Action<float> progress = null);

        bool SaveTracks(IEnumerable<T> spotifyTracks, int delay);

        Task<bool> SaveTracksAsync(IEnumerable<T> spotifyTracks, CancellationToken ct);
    }
}