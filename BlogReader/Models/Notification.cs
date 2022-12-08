using BlogReader.Models.Enums;

namespace BlogReader.Models
{
    public class Notification : BaseEntity
    {
        private string _message;
        private MessageType _messageType;

        public string Message
        {
            get { return _message; }
            set 
            {
                _message = value; 
                OnPropertyChanged(nameof(Message));
            }
        }

        public MessageType MessageType 
        {
            get { return _messageType; }
            set 
            {
                _messageType = value;
                OnPropertyChanged(nameof(MessageType));
            }
        }

        public Notification() { }

        public Notification(string message, MessageType messageType)
        {
            _message = message;
            _messageType = messageType;
        }
    }
}
