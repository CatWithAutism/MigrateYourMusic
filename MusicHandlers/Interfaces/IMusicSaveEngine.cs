using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicHandlers.Models;

namespace MusicHandlers.Interfaces
{
    public interface IMusicSaveEngine<in TIn> where TIn : Track
    {
        bool SaveTracks(IEnumerable<TIn> tracks, int delay);

        Task<bool> SaveTracksAsync(IEnumerable<TIn> tracks, CancellationToken ct);
    }
}