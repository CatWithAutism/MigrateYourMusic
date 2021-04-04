using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicCarriers.Engines.SearchEngines.Spotify;
using MusicCarriers.Models;
using MusicCarriers.Utils;

namespace Tests.SpotifyTests
{
    [TestClass]
    public class SpotifyTests
    {
        private readonly SpotifySearchEngine<Track> _searchEngine;

        public SpotifyTests()
        {
            var initData = Utils.ReadFile(@$"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}{BasicInfo.Default_init_data}");
            
            var clientId = initData.GetKey("SPOTIFY.CLIENT_ID");
            var secretId = initData.GetKey("SPOTIFY.SECRET_ID");
            
            Assert.IsNotNull(clientId);
            Assert.IsNotNull(secretId);
            
            var spotifyClient = SpotifyUtils.GetAuthorizedByIds(clientId, secretId);
            _searchEngine = new SpotifySearchEngine<Track>(spotifyClient);
            
            Assert.IsNotNull(_searchEngine);
        }
        
        [TestMethod]
        public void SearchTest()
        {
            lock (_searchEngine)
            {
                var song = _searchEngine.FindTrackPair(new Track
                {
                    Artist = "Fun Mode",
                    Title = "Стены Цитадели"
                });

                Assert.IsNotNull(song);
                Assert.AreEqual(song.SpotifyUri, "spotify:track:0IdRmWk5hmbETbfxvAZfzw");
            }
        }
        
        
    }
}