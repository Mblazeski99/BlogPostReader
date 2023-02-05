using BlogReader.Commands;

namespace BlogReader.Models
{
    public class NavigationMenuItem : BaseModel
    {
        private string _title = string.Empty;
        private string _icon = string.Empty;
        private bool _isExitItem = false;
        private bool _isSelected = false;
        private BaseCommand _command = null;
        private string _commandParameter = string.Empty;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public string Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }

        public bool IsExitItem
        {
            get { return _isExitItem; }
            set
            {
                _isExitItem = value;
                OnPropertyChanged(nameof(IsExitItem));
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public BaseCommand Command
        {
            get { return _command; }
            set
            {
                _command = value;
                OnPropertyChanged(nameof(Command));
            }
        }

        public string CommandParameter
        {
            get { return _commandParameter; }
            set
            {
                _commandParameter = value;
                OnPropertyChanged(nameof(CommandParameter));
            }
        }
    }
}
