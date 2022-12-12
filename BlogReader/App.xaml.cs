using BlogReader.HostBuilders;
using BlogReader.Models;
using BlogReader.Models.Enums;
using BlogReader.Stores;
using BlogReader.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;
using System.Windows.Documents;
using ToastNotifications;
using ToastNotifications.Core;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace BlogReader
{
    public partial class App : Application
    {
        private readonly IHost _host;
        private NotificationsStore _notificationsStore;
        private BlogPostItemsStore _blogPostItemsStore;
        private Notifier _notifier;
        private AppData _appData;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .AddStores()
                .AddViewModels()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(s => new MainWindow()
                    {
                        DataContext = s.GetRequiredService<MainWindowViewModel>()
                    });
            }).Build();

            _appData = new AppData();
            InitializeNotifier();
            InitializeGlobalEvents();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            var navigationStore = _host.Services.GetRequiredService<NavigationStore>();
            navigationStore.CurrentViewModel = new HomeViewModel();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        private void InitializeNotifier()
        {
            var notifierMessageOptions = new MessageOptions() { UnfreezeOnMouseLeave = true };

            _notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: Current.MainWindow,
                    corner: Corner.TopLeft,
                    offsetX: 0,
                    offsetY: 0);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(5),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                cfg.Dispatcher = Current.Dispatcher;
            });

            _blogPostItemsStore = _host.Services.GetRequiredService<BlogPostItemsStore>();
            _notificationsStore = _host.Services.GetRequiredService<NotificationsStore>();
            _notificationsStore.NotificationAdded += (currentNotification, args) =>
            {
                var notification = currentNotification as Notification;

                switch (notification?.MessageType)
                {
                    case MessageType.Error:
                        _notifier.ShowError(notification.Message, notifierMessageOptions);
                        break;

                    case MessageType.Status:
                        _notifier.ShowInformation(notification.Message, notifierMessageOptions);
                        break;

                    case MessageType.Success:
                        _notifier.ShowSuccess(notification.Message, notifierMessageOptions);
                        break;
                }
            };
        }

        private void InitializeGlobalEvents()
        {
            EventManager.RegisterClassHandler(typeof(Hyperlink),
                Hyperlink.ClickEvent,
                new RoutedEventHandler(_appData.Hyperlink_RequestNavigate));
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.Dispose();
            _notificationsStore.Dispose();
            _blogPostItemsStore.Dispose();
            base.OnExit(e);
        }
    }
}
