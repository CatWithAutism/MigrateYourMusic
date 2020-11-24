using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;

using System.IO;
using System.Linq;
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
        private static string _clientId;
        private static string _secretId;

        /// <summary>
        /// Авторизация API.
        /// </summary>
        static VKTests()
        {
            var iniData = Utils.ReadFile(@"C:\Users\vlad3\source\repos\SpotifyHelper\SpotifyHelper\SpotifyHandlerConfig.ini");
            _login = iniData.GetKey("VK.LOGIN");
            _password = iniData.GetKey("VK.PASSWORD");
            _screenName = iniData.GetKey("VK.SCREEN_NAME");

            _clientId = iniData.GetKey("SPOTIFY.CLIENT_ID");
            _secretId = iniData.GetKey("SPOTIFY.SECRET_ID");

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
            VkTrackListDownloader downloader = new VkTrackListDownloader(_api, 6000);
            var trackList = downloader.DownloadTrackList(user);
            Assert.AreNotEqual(null, trackList);
        }

        [TestMethod]
        public void GetSongPairs()
        {
            var user = VKUtils.GetUserByScreenName(_screenName, _api);
            VkTrackListDownloader downloader = new VkTrackListDownloader(_api, 6000);
            var trackList = downloader.DownloadTrackList(user);

            SpotifyHandler handler = SpotifyHandler.GetAuthorizedByIds(_clientId, _secretId);

            var tracksPairs = handler.FindTracksPairs(trackList, 100, new System.Threading.CancellationToken(), null);

            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach(var pair in tracksPairs)
            {
                i++;
                sb.Append($"{i}. VK: {pair.Key.Artist} - {pair.Key.Title} | ID:{pair.Value.Id} Spotify: {pair.Value.Artist} - {pair.Value.Title}, " +
                    $"{pair.Value.Album}, {pair.Value.AlbumPictureUri}, {pair.Value.Artist}, {pair.Value.SpotifyUri}, {pair.Value.Duration} " +
                    $"| Similarity: {(int)(JaroWinkler.Similarity($"{pair.Key.Artist} - {pair.Key.Title}", $"{pair.Value.Artist} - {pair.Value.Title}") * 100)}%\r\n");
            }
            var d = Newtonsoft.Json.JsonConvert.SerializeObject(tracksPairs.ToList());
            File.WriteAllText(@"C:\Users\vlad3\Desktop\ResultJson.txt", d);
            File.WriteAllText(@"C:\Users\vlad3\Desktop\Result1.txt", sb.ToString());
        }
    }
}
