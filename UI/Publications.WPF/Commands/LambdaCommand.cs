using System;
using Publications.WPF.Commands.Base;

namespace Publications.WPF.Commands;

public class LambdaCommand : Command
{
    private readonly Action<object?> _Execute;
    private readonly Func<object?, bool>? _CanExecute;

    public LambdaCommand(Action<object?> Execute, Func<object?, bool>? CanExecute = null)
    {
        //if(Execute is null)
        //    throw new ArgumentNullException(nameof(Execute));
        //_Execute = Execute;

        _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
        _CanExecute = CanExecute;
    }

    public override bool CanExecute(object? parameter) => 
        base.CanExecute(parameter) 
        && (_CanExecute?.Invoke(parameter) ?? true);

    public override void Execute(object? parameter) => _Execute(parameter);
}