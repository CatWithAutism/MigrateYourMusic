using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VkNet.Model;
using WebHandlers.Downloaders.VK;
using WebHandlers.Interfaces;
using WebHandlers.Models;
using WebHandlers.Utils;

namespace MigrateYourMusic.Services
{
    public static class ServicesExtension
    {
        public static void AddVkTrackListDownloader(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            serviceCollection.AddSingleton<ITrackListDownloader<VkTrack, User>>(new VkTrackListDownloader(
                VkUtils.AuthorizeApi(configuration["VK:LOGIN"], configuration["VK:PASSWORD"]),
                uint.Parse(configuration["VK:MAX_REQUEST_LENGTH"])));
        }
    }
}