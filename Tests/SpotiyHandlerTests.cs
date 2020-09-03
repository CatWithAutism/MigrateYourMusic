using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Text;

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
            var song = SpotifyHandler.FindTrackPair(new Track { Artist = "Louna", Title = "Огня" });
        }
    }
}
