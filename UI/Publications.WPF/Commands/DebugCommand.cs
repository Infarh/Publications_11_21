using System;
using System.Diagnostics;
using System.Windows.Input;
using Publications.WPF.Commands.Base;

namespace Publications.WPF.Commands;

public class DebugCommand : Command, ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add => _BaseCommand.CanExecuteChanged += value;
        remove => _BaseCommand.CanExecuteChanged -= value;
    }

    private readonly ICommand _BaseCommand;

    public DebugCommand(ICommand BaseCommand)
    {
        _BaseCommand = BaseCommand is DebugCommand debug_command 
            ? debug_command._BaseCommand
            : BaseCommand;
    }

    public override bool CanExecute(object? parameter)
    {
        var can_execute = _BaseCommand.CanExecute(parameter);
        return can_execute;
    }

    public override void Execute(object? parameter)
    {
        Debug.WriteLine("Вызов команды {0} {1}", _BaseCommand.GetHashCode(), _BaseCommand.GetType());
        var timer = Stopwatch.StartNew();
        _BaseCommand.Execute(parameter);
        Debug.WriteLine("Вызов команды {0} {1} завершён за {2:0.00} c", 
            _BaseCommand.GetHashCode(), _BaseCommand.GetType(),
            timer.Elapsed.TotalSeconds);
    }
}