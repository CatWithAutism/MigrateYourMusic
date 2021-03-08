using System;
using System.Threading.Tasks;
using MusicHandlers.Interfaces;
using MusicHandlers.Models;

namespace MusicHandlers
{
    public class MusicCarrier<T, V> 
        where T : Track 
        where V : Track
    {
        private readonly IMusicSaveEngine<T> _musicSaveEngine;
        private readonly IMusicSearchEngine<T, V> _searchEngine;
        
        public MusicCarrier(IMusicSearchEngine<T, V> searchEngine, IMusicSaveEngine<T> musicSaveEngine)
        {
            _searchEngine = searchEngine;
            _musicSaveEngine = musicSaveEngine;
        }

        public async Task<bool> TransferMusic()
        {
            throw new NotImplementedException();
        }

    }
}