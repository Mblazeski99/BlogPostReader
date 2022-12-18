using BlogReader.Models.Enums;

namespace BlogReader.Models
{
    public class Notification : BaseEntity
    {
        private string _message;
        private string _exception;
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

        public string Exception
        {
            get { return _exception; }
            set 
            {
                _exception = value; 
                OnPropertyChanged(nameof(Exception));
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

        public Notification(MessageType messageType, string message, string exception = null)
        {
            _message = message;
            _messageType = messageType;
            _exception = exception;
        }
    }
}
