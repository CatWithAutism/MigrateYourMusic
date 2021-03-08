using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicHandlers.Models;

namespace MusicHandlers.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TOut">What you want to find</typeparam>
    /// <typeparam name="TIn">What you want to find it through</typeparam>
    public interface IMusicSearchEngine<TOut, in TIn> 
        where TOut : Track
        where TIn : Track
    {
        TOut FindTrackPair(TIn track);

        Task<TOut> FindTrackPairAsync(TIn track, CancellationToken ct);

        IEnumerable<TOut> FindTracksPairs(IEnumerable<TIn> tracks, Action<float> progress = null);

        Task<IEnumerable<TOut>> FindTracksPairsAsync(IEnumerable<TIn> tracks, CancellationToken ct,
            Action<float> progress = null);
    }
}