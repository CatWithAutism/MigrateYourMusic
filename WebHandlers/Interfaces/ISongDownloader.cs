using System;
using System.Collections.Generic;
using System.Text;

using WebHandlers.Models;

namespace WebHandlers.Interfaces
{
    public interface ITrackListDownloader<T>
    {
        IEnumerable<Track> DownloadTrackList(T param);
    }
}
