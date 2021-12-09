using System;
using System.Windows.Input;

namespace Publications.WPF.Commands.Base;

public class CommandBuilder
{
    private Action<object?>? _Execute;
    private Func<object?, bool>? _CanExecute;
    private bool _Debug;

    public CommandBuilder() { }

    public CommandBuilder(Action<object?> Execute) => _Execute = Execute;

    public CommandBuilder When(Func<object?, bool> CanExecute)
    {
        _CanExecute = CanExecute;
        return this;
    }

    public CommandBuilder Invoke(Action<object?> Execute)
    {
        _Execute += Execute;
        return this;
    }

    public CommandBuilder Debug()
    {
        _Debug = true;
        return this;
    }

    public ICommand Build()
    {
        var result = new LambdaCommand(_Execute!, _CanExecute);
        return _Debug ? new DebugCommand(result) : result;
    }

    public static implicit operator Command(CommandBuilder builder) => (Command)builder.Build();
}