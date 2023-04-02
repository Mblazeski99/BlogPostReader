using System;
using System.ComponentModel;

namespace BlogReader.DataModels.Interfaces
{
    public interface IBaseEntity
    {
        DateTime? DateCreated { get; set; }
        DateTime? DateModified { get; set; }
        string Id { get; set; }

        event PropertyChangedEventHandler? PropertyChanged;

        void Dispose();
    }
}