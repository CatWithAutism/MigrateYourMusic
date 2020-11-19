using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using WebHandlers.Models;

namespace WebHandlers.Interfaces
{
    public interface ITrackSearchEngine<T> where T : class
    {
        SpotifyTrack FindTrackPair(Track track);

        Task<T> FindTrackPairAsync(Track track);

        Dictionary<Track, T> FindTracksPairs(IEnumerable<Track> tracks);

        Task<Dictionary<Track, T>> FindTracksPairsAsync(IEnumerable<Track> tracks);
    }
}
