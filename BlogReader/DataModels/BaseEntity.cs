using System;
using System.ComponentModel;
using BlogReader.DataModels.Interfaces;

namespace BlogReader.DataModels
{
    public abstract class BaseEntity : INotifyPropertyChanged, IBaseEntity
    {
        private string _id;
        private DateTime? _dateCreated;
        private DateTime? _dateModified;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public DateTime? DateCreated
        {
            get { return _dateCreated; }
            set
            {
                _dateCreated = value;
                OnPropertyChanged(nameof(DateCreated));
            }
        }

        public DateTime? DateModified
        {
            get { return _dateModified; }
            set { _dateModified = value; }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public BaseEntity()
        {
            _id = Guid.NewGuid().ToString();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (DateCreated.HasValue)
            {
                DateModified = DateTime.Now;
            }
        }

        public virtual void Dispose() { }
    }
}
