using BlogReader.HostBuilders;
using BlogReader.DataModels;
using BlogReader.DataModels.Enums;
using BlogReader.Stores;
using BlogReader.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
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
        private MessageOptions _notifierMessageOptions;
        private NotificationsStore _notificationsStore;
        private BlogPostItemsStore _blogPostItemsStore;
        private Notifier _notifier;

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

            Current.Dispatcher.BeginInvoke(() =>
            {
                InitializeNotifier();
            });

            InitializeGlobalEvents();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            var navigationStore = _host.Services.GetRequiredService<NavigationStore>();
            _blogPostItemsStore = _host.Services.GetRequiredService<BlogPostItemsStore>();
            _notificationsStore = _host.Services.GetRequiredService<NotificationsStore>();

            navigationStore.CurrentViewModel = new HomeViewModel(_blogPostItemsStore, _notificationsStore);

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        private void InitializeNotifier()
        {
            _notifierMessageOptions = new MessageOptions() { UnfreezeOnMouseLeave = true };

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

            _blogPostItemsStore.OnException += (messageObj, args) =>
            {
                string message = messageObj?.ToString();
                string exception = (args as ErrorEventArgs)?.GetException().ToString();

                var notification = new Notification(MessageType.Error, message, exception);
                ShowNotification(notification);
            };

            _notificationsStore.NotificationAdded += (currentNotification, args) => ShowNotification(currentNotification as Notification);
        }

        private void InitializeGlobalEvents()
        {
            EventManager.RegisterClassHandler(typeof(Hyperlink),
                Hyperlink.ClickEvent,
                new RoutedEventHandler(AppData.Hyperlink_RequestNavigate));

            EventManager.RegisterClassHandler(typeof(TextBox),
                TextBox.GotKeyboardFocusEvent,
                new KeyboardFocusChangedEventHandler(AppData.TextBox_GotKeyboardFocus));

            EventManager.RegisterClassHandler(typeof(TextBox),
                TextBox.PreviewMouseDownEvent,
                new MouseButtonEventHandler(AppData.TextBox_PreviewMouseDown));

            EventManager.RegisterClassHandler(typeof(TextBlock),
                TextBlock.PreviewMouseLeftButtonUpEvent,
                new MouseButtonEventHandler(AppData.HyperLinkTextBlock_PreviewMouseLeftButtonUp));
        }

        private void ShowNotification(Notification notification)
        {
            switch (notification?.MessageType)
            {
                case MessageType.Error:
                    _notifier.ShowError(notification.Message, _notifierMessageOptions);
                    break;

                case MessageType.Status:
                    _notifier.ShowInformation(notification.Message, _notifierMessageOptions);
                    break;

                case MessageType.Success:
                    _notifier.ShowSuccess(notification.Message, _notifierMessageOptions);
                    break;
            }
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
