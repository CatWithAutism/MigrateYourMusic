using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebHandlers.Models;

namespace WebHandlers.Interfaces
{
    public interface IWebHandler<T> where T : class
    {
        T FindTrackPair(Track track);

        Task<T> FindTrackPairAsync(Track track, CancellationToken ct);

        Dictionary<Track, T> FindTracksPairs(IEnumerable<Track> tracks, int delay, Action<float> progress = null);

        Task<Dictionary<Track, T>> FindTracksPairsAsync(IEnumerable<Track> tracks, int delay, CancellationToken ct,
            Action<float> progress = null);

        bool SaveTracks(IEnumerable<T> spotifyTracks, int delay);

        Task<bool> SaveTracksAsync(IEnumerable<T> spotifyTracks, int delay, CancellationToken ct);
    }
}