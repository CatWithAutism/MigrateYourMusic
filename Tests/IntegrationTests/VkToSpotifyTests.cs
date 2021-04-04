using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicCarriers.Engines.DownloadEngines.VK;
using MusicCarriers.Engines.SearchEngines.Spotify;
using MusicCarriers.Models;
using VkNet;
using MusicCarriers.Utils;

namespace Tests.IntegrationTests
{
    [TestClass]
    public class VkToSpotifyTests
    {
        private readonly SpotifySearchEngine<Track> _searchEngine;
        private readonly VkApi _api;
        private readonly string _screenName;
        
        public VkToSpotifyTests()
        {
            var initData = Utils.ReadFile(@$"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}{BasicInfo.Default_init_data}");
            
            var login = initData.GetKey("VK.LOGIN");
            var password = initData.GetKey("VK.PASSWORD");
            _screenName = initData.GetKey("VK.SCREEN_NAME");
            
            Assert.IsNotNull(login);
            Assert.IsNotNull(password);
            Assert.IsNotNull(_screenName);

            _api = VkUtils.AuthorizeApi(login, password);
            
            Assert.IsNotNull(_api);
            Assert.IsTrue(_api.IsAuthorized);
            
            var clientId = initData.GetKey("SPOTIFY.CLIENT_ID");
            var secretId = initData.GetKey("SPOTIFY.SECRET_ID");
            
            Assert.IsNotNull(clientId);
            Assert.IsNotNull(secretId);

            var spotifyClient = SpotifyUtils.GetAuthorizedByIds(clientId, secretId);
            _searchEngine = new SpotifySearchEngine<Track>(spotifyClient);
            
            Assert.IsNotNull(_searchEngine);
        }
        
        [TestMethod]
        public void GetSongPairs()
        {
            lock (_searchEngine)
            {
                var user = VkUtils.GetUserByScreenName(_screenName, _api);
                Assert.IsNotNull(user);
                
                var downloader = new VkMusicDownloadEngine(_api, 6000);
                var trackList = downloader.DownloadTrackList(user);
                
                Assert.IsNotNull(trackList);

                var tracksPairs = _searchEngine.FindTracksPairs(trackList);
                
                Assert.IsNotNull(tracksPairs);
            }
        }
    }
}