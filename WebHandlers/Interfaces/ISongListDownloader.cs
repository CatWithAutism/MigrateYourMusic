using System;
using System.Collections.Generic;
using System.Text;

using WebHandlers.Models;

namespace WebHandlers.Interfaces
{
    public interface ISongListDownloader<T>
    {
        IEnumerable<Track> GetTrackList(T param);
    }
}
