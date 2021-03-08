using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VkNet;
using WebHandlers.Downloaders.VK;
using WebHandlers.Handlers.Spotify;
using WebHandlers.Utils;

namespace Tests.IntegrationTests
{
    [TestClass]
    public class VkToSpotifyTests
    {
        private readonly SpotifyHandler _handler;
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
            
            _handler = SpotifyHandler.GetAuthorizedByIds(clientId, secretId);
            
            Assert.IsNotNull(_handler);
        }
        
        [TestMethod]
        public void GetSongPairs()
        {
            lock (_handler)
            {
                var user = VkUtils.GetUserByScreenName(_screenName, _api);
                Assert.IsNotNull(user);
                
                var downloader = new VkTrackListDownloader(_api, 6000);
                var trackList = downloader.DownloadTrackList(user);
                
                Assert.IsNotNull(trackList);

                var tracksPairs = _handler.FindTracksPairs(trackList);
                
                Assert.IsNotNull(tracksPairs);
            }
        }
    }
}