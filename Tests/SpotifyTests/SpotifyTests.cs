using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebHandlers.Handlers.Spotify;
using WebHandlers.Models;

namespace Tests.SpotifyTests
{
    [TestClass]
    public class SpotifyTests
    {
        private readonly SpotifyHandler _handler;

        public SpotifyTests()
        {
            var initData = Utils.ReadFile(@$"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}{BasicInfo.Default_init_data}");
            
            var clientId = initData.GetKey("SPOTIFY.CLIENT_ID");
            var secretId = initData.GetKey("SPOTIFY.SECRET_ID");
            
            Assert.IsNotNull(clientId);
            Assert.IsNotNull(secretId);
            
            _handler = SpotifyHandler.GetAuthorizedByIds(clientId, secretId);
            
            Assert.IsNotNull(_handler);
        }
        
        [TestMethod]
        public void SearchTest()
        {
            lock (_handler)
            {
                var song = _handler.FindTrackPair(new Track
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