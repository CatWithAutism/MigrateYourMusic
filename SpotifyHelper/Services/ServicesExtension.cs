using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicHandlers.DownloadEngines.VK;
using VkNet.Model;
using MusicHandlers.Interfaces;
using MusicHandlers.Models;
using MusicHandlers.Utils;

namespace MigrateYourMusic.Services
{
    public static class ServicesExtension
    {
        public static void AddVkTrackListDownloader(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            serviceCollection.AddSingleton<IMusicDownloadEngine<VkTrack, User>>(new VkMusicDownloadEngine(
                VkUtils.AuthorizeApi(configuration["VK:LOGIN"], configuration["VK:PASSWORD"]),
                uint.Parse(configuration["VK:MAX_REQUEST_LENGTH"])));
        }
    }
}