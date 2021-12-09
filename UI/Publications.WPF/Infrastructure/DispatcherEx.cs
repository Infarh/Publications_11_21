using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Threading;

internal static class DispatcherEx
{
    public static DispatcherAwaiter GetAwaiter(this Dispatcher dispatcher)
    {
        return new(dispatcher);
    }
}

public readonly struct DispatcherAwaiter : INotifyCompletion
{
    private readonly DispatcherPriority _Priority;
    private readonly Dispatcher _Dispatcher;

    public bool IsCompleted => _Dispatcher.CheckAccess();

    public DispatcherAwaiter(Dispatcher dispatcher)
    {
        _Dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        _Priority = DispatcherPriority.Normal;
    }

    public DispatcherAwaiter(Dispatcher dispatcher, DispatcherPriority Priority)
    {
        _Dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        _Priority = Priority;
    }

    public void OnCompleted(Action continuation)
    {
        if (_Priority == DispatcherPriority.Normal)
            _Dispatcher.Invoke(continuation);
        else
            _Dispatcher.Invoke(continuation, _Priority);
    }

    public void GetResult() { }
}