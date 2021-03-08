using System.Collections.Generic;

namespace MusicHandlers.Interfaces
{
    public interface IMusicDownloadEngine<out TOut,in TIn>
    {
        public IEnumerable<TOut> DownloadTrackList(TIn param);
    }
}