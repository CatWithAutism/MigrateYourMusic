using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SpotifyHelper.Pages
{
    public class IndexModel : PageModel
    {
        [ViewData]
        public string AppAuthLink { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration _configuration)
        {
            _logger = logger;
            AppAuthLink = _configuration["Spotify:APP_AUTH_LINK"];
        }

        public void OnGet()
        {

        }
    }
}
