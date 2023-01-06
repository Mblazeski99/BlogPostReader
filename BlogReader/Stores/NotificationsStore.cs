using System;
using BlogReader.Models;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace BlogReader.Stores
{
    public class NotificationsStore : BaseStore
    {
        private readonly string _notificationsFilePath = String.Empty;
        private readonly ObservableCollection<Notification> _notifications;

        public ObservableCollection<Notification> Notifications => _notifications;
        public event Action NotificationsChanged;
        public event EventHandler NotificationAdded;

        public NotificationsStore() : base()
        {
            _notificationsFilePath = DataItemsFolderPath + @"\Notifications.txt";
            _notifications = new ObservableCollection<Notification>();

            if (File.Exists(_notificationsFilePath))
            {
                using (StreamReader sr = new StreamReader(_notificationsFilePath))
                {
                    string notificationsJson = sr.ReadToEnd();
                    if (!String.IsNullOrEmpty(notificationsJson))
                    {
                        _notifications = JsonConvert.DeserializeObject<ObservableCollection<Notification>>(notificationsJson);
                    }
                }
            }
            else
            {
                using (FileStream fs = File.Create(_notificationsFilePath)) { }
            }
        }

        public void AddNotification(Notification notification)
        {
            notification.DateCreated = DateTime.Now;
            _notifications.Add(notification);

            NotificationAdded?.Invoke(notification, new EventArgs());
            NotificationsChanged?.Invoke();
        }

        public void ClearNotifications()
        {
            _notifications?.Clear();
            NotificationsChanged?.Invoke();
        }

        public override void Dispose()
        {
            SaveItemsToFile(_notificationsFilePath, _notifications.ToList());
            base.Dispose();
        }
    }
}
