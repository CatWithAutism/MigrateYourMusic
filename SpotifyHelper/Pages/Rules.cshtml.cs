using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SpotifyAPI.Web;
using WebHandlers.Utils;

namespace MigrateYourMusic.Pages
{
    public class RulesModel : PageModel
    {
        [ViewData]
        public string AppAuthLink { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public RulesModel(ILogger<IndexModel> logger, IConfiguration _configuration)
        {
            _logger = logger;

            string url = _configuration["MAIN:SPOTIFY_CALLBACK"];
            string clientId = _configuration["SPOTIFY:CLIENT_ID"];

            Guarantee.IsStringNotNullOrEmpty(url, nameof(url));
            Guarantee.IsStringNotNullOrEmpty(clientId, nameof(clientId));

            AppAuthLink = SpotifyUtils.GetLoginUri(url, clientId, LoginRequest.ResponseType.Code, Scopes.UserReadPrivate, Scopes.UserLibraryModify);
        }

        public void OnGet()
        {

        }
    }
}
