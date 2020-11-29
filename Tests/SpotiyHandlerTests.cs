using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebHandlers.Handlers.Spotify;
using WebHandlers.Models;

namespace Tests
{
    [TestClass]
    public class SpotiyHandlerTests
    {
        [TestMethod]
        public void SearchTest()
        {
            var iniData = Utils.ReadFile(@"C:\Users\vlad3\source\repos\SpotifyHelper\SpotifyHelper\SpotifyHandlerConfig.ini");
            string clientId = iniData.GetKey("SPOTIFY.CLIENT_ID");
            string secretId = iniData.GetKey("SPOTIFY.SECRET_ID");

            SpotifyHandler handler = SpotifyHandler.GetAuthorizedByIds(clientId, secretId);
            var song = handler.FindTrackPair(new Track { Artist = "Louna", Title = "Огня" });

            Assert.IsNotNull(song);
        }
    }
}
