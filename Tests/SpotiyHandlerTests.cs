using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Text;

using WebHandlers.SearchEngines.Spotify;
using WebHandlers.Models;
using WebHandlers.Utils;

namespace Tests
{
    [TestClass]
    public class SpotiyHandlerTests
    {
        [TestMethod]
        public void SearchTest()
        {
            var song = SpotifySearchEngine.FindTrackPair(new Track { Artist = "Louna", Title = "Огня" });
        }
    }
}
