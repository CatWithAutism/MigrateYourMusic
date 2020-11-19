using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.IO;
using System.Text;

using VkNet;
using VkNet.Model;

using WebHandlers;
using WebHandlers.Downloaders.VK;
using WebHandlers.Handlers.Spotify;
using WebHandlers.Utils;

namespace Tests
{
    [TestClass]
    public class VKTests
    {
        private static VkApi _api;
        private static string _screenName;
        private static string _login;
        private static string _password;


        /// <summary>
        /// Авторизация API.
        /// </summary>
        static VKTests()
        {
            _login = "79015015323";
            _password = "TestBotVKPassword10!";
            _screenName = "dzargevich";

            //Тестам в любом случае нужна авторизация.
            _api = VKUtils.AuthorizeApi(_login, _password);
            Assert.IsTrue(_api.IsAuthorized);
        }

        /// <summary>
        /// Получение пользователя по его screen name(url).
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void GetUserByScreenNameTest()
        {
            var user = VKUtils.GetUserByScreenName(_screenName, _api);
            Assert.AreNotEqual(null, user);
        }

        /// <summary>
        /// Получение списка треков пользователя.
        /// </summary>
        [TestMethod]
        public void GetAudioVKTest()
        {
            var user = VKUtils.GetUserByScreenName(_screenName, _api);
            VkTrackListDownloader downloader = new VkTrackListDownloader(_api);
            var trackList = downloader.DownloadTrackList(user);
            Assert.AreNotEqual(null, trackList);
        }

        [TestMethod]
        public void GetSongPairs()
        {
            var user = VKUtils.GetUserByScreenName(_screenName, _api);
            VkTrackListDownloader downloader = new VkTrackListDownloader(_api);
            var trackList = downloader.DownloadTrackList(user);

            var d = SpotifySearchEngine.FindTracksPairs(trackList);

            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach(var c in d)
            {
                i++;
                sb.Append($"{i}. VK: {c.Key.Artist} - {c.Key.Title} | Spotify: {c.Value.Artist} - {c.Value.Title}, " +
                    $"{c.Value.Album}, {c.Value.AlbumPicture}, {c.Value.Artist}, {c.Value.SpotifyUri}, {c.Value.Duration}\r\n");
            }
            File.WriteAllText(@"C:\Users\vlad3\Desktop\Result1.txt", sb.ToString());
        }
    }
}
