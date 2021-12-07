using System;
using System.Windows.Input;

namespace Publications.WPF.Commands.Base;

public abstract class Command : ICommand
{
    public static CommandBuilder Invoke(Action<object?> Execute) => new(Execute);

    public static CommandBuilder New() => new();

    public static ICommand New(Action<object?> Execute, Func<object?, bool>? CanExecute = null) => new LambdaCommand(Execute, CanExecute);

    public static ICommand New(Action Execute, Func<bool>? CanExecute = null)
    {
        Action<object?> execute = _ => Execute();
        Func<object?, bool>? can_execute = CanExecute is null ? null : _ => CanExecute();
        return new LambdaCommand(execute, can_execute);
    }

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