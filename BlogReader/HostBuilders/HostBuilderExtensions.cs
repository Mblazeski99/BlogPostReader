using BlogReader.Services;
using BlogReader.Services.Interfaces;
using BlogReader.Stores;
using BlogReader.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlogReader.HostBuilders
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<HomeViewModel>();
                services.AddSingleton<NotificationsLogViewModel>();
            });

            return hostBuilder;
        }

        public static IHostBuilder AddServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<IBlogPostItemService, BlogPostItemService>();
            });

            return hostBuilder;
        }

        public static IHostBuilder AddStores(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<NavigationStore>();
                services.AddSingleton<NotificationsStore>();
                services.AddSingleton<BlogPostItemsStore>();
            });

            return hostBuilder;
        }
    }
}
