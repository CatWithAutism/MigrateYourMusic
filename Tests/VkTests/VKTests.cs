using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VkNet;
using WebHandlers.Downloaders.VK;
using WebHandlers.Utils;

namespace Tests.VkTests
{
    [TestClass]
    public class VkTests
    {
        private readonly VkApi _api;
        private readonly string _screenName;

        /// <summary>
        ///     API Authorization
        /// </summary>
        public VkTests()
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
        }

        /// <summary>
        ///     Get the user by screen name
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void GetUserByScreenNameTest()
        {
            lock (_api)
            {
                var user = VkUtils.GetUserByScreenName(_screenName, _api);
                Assert.IsNotNull(user);
            }
        }

        /// <summary>
        ///     Getting track list
        /// </summary>
        [TestMethod]
        public void GetAudioVkTest()
        {
            lock (_api)
            {
                var user = VkUtils.GetUserByScreenName(_screenName, _api);
                var downloader = new VkTrackListDownloader(_api, 6000);
                var trackList = downloader.DownloadTrackList(user);
                
                Assert.IsNotNull(trackList);
                Assert.AreNotEqual(0, trackList.Count());
            }
        }
    }
}