using System;
using System.Windows.Input;

namespace Publications.WPF.Commands.Base
{
    public abstract class Command : ICommand
    {
        //private event EventHandler? _CanExecuteChanged;
        public event EventHandler? CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                //_CanExecuteChanged += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
                //_CanExecuteChanged -= value;
            }
        }

        //protected virtual void OnCanExecuteChanged(EventArgs e) => _CanExecuteChanged?.Invoke(this, e);

        public virtual bool CanExecute(object? parameter) => true;

        public abstract void Execute(object? parameter);
    }
}
