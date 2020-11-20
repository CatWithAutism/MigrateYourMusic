using System;
using System.Collections.Generic;
using System.Text;

using WebHandlers.Models;

namespace WebHandlers.Interfaces
{
    public interface ITrackListDownloader<out T, in V> where T: Track
    {
        IEnumerable<T> DownloadTrackList(V param);
    }
}
