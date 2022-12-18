using System;
using System.Windows.Input;

namespace BlogReader.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public event EventHandler OnExecuted;

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public virtual void Execute(object parameter)
        {
            OnExecuted?.Invoke(parameter, new EventArgs());
        }

        protected void OnCanExecutedChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public virtual void Dispose() { }
    }
}
