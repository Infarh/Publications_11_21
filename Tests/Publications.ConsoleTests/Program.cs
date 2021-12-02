using System.Collections.Concurrent;
using System.Diagnostics;

using Microsoft.Extensions.DependencyInjection;

namespace Publications.ConsoleTests;

interface IPrinter
{
    void Print(string str);
}

enum PrinterType : byte
{
    Console,
    Debug,
    Trace
}

abstract class Printer : IPrinter
{
    private static readonly ConcurrentDictionary<PrinterType, Printer> __Printers = new();

    //public static Printer Cerate(PrinterType type)
    //{
    //    if (__Printers.TryGetValue(type, out var printer))
    //        return printer;

    //    switch (type)
    //    {
    //        default: throw new ArgumentOutOfRangeException(nameof(type));

    //        case PrinterType.Console:
    //            printer = new ConsolePrinter();
    //            break;

    //        case PrinterType.Debug:
    //            printer = new DebugPrinter();
    //            break;

    //        case PrinterType.Trace:
    //            printer = new TracePrinter();
    //            break;

    //    }

    //    __Printers[type] = printer;
    //    return printer;
    //}

    public static Printer Cerate(PrinterType type) => __Printers.GetOrAdd(type, t => t switch
    {
        PrinterType.Console => new ConsolePrinter(),
        PrinterType.Debug => new DebugPrinter(),
        PrinterType.Trace => new TracePrinter(),
        _ => throw new ArgumentOutOfRangeException(nameof(type))
    });

    public abstract void Print(string msg);
}

class DebugPrinter : Printer
{
    public override void Print(string msg) => Debug.WriteLine("{0:yyyy-MM-dd HH-mm-ss.ffff}> {1}", DateTime.Now, msg);
}

class TracePrinter : Printer
{
    public override void Print(string msg) => Trace.Write($"{DateTime.Now:yyyy-MM-dd HH-mm-ss.ffff}> {msg}");
}

class ConsolePrinter : Printer
{
    public override void Print(string msg) => Console.WriteLine("{0:yyyy-MM-dd HH-mm-ss.ffff}> {1}", DateTime.Now, msg);
}

interface IDataProcessor
{
    void Process(double value);
}

class SimpleValueProcessor : IDataProcessor
{
    private readonly IPrinter _Printer;

    public SimpleValueProcessor(IPrinter Printer)
    {
        _Printer = Printer;
    }

    public void Process(double value)
    {
        _Printer.Print(value.ToString());
    }

}

class Program
{
    public static async Task Main(string[] args)
    {
        var console_printer = Printer.Cerate(PrinterType.Console);
        var debug_printer = Printer.Cerate(PrinterType.Debug);
        var trace_printer = Printer.Cerate(PrinterType.Trace);

        var services = new ServiceCollection();
        services.AddSingleton<IPrinter, ConsolePrinter>();
        services.AddScoped<IDataProcessor, SimpleValueProcessor>();

        IServiceProvider provider = services.BuildServiceProvider();
        IServiceProvider provider2 = services.BuildServiceProvider();

        var is_different = !ReferenceEquals(provider, provider2);

        var processor = provider.GetRequiredService<IDataProcessor>();

        processor.Process(3.1415926535897932);
    }

    private static async Task<int> LongCalculation(int IterationsCount, int Timeout, CancellationToken Cancel = default)
    {
        //await Task.Delay(1).ConfigureAwait(false);
        //await Task.CompletedTask.ConfigureAwait(false);
        //await Task.Run(() => { }).ConfigureAwait(false);

        await Task.Yield().ConfigureAwait(false);

        //return await Task.Run(async () =>
        //{
        //    var result = 0;
        //    for (var i = 1; i < IterationsCount; i++)
        //    {
        //        Cancel.ThrowIfCancellationRequested();
        //        result += i;
        //        await Task.Delay(Timeout).ConfigureAwait(false);
        //    }

        //    return result;
        //});

        var result = 0;
        for (var i = 1; i < IterationsCount; i++)
        {
            Cancel.ThrowIfCancellationRequested();
            result += i;
            await Task.Delay(Timeout).ConfigureAwait(false);
        }

        return result;
    }
}