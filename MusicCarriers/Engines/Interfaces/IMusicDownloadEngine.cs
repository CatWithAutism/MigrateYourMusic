using System.Collections.Generic;

namespace MusicCarriers.Engines.Interfaces
{
    public interface IMusicDownloadEngine<out TOut,in TIn>
    {
        public IEnumerable<TOut> DownloadTrackList(TIn param);
    }
}