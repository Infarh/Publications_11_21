using System.Threading;
using System.Diagnostics;

namespace Publications.ConsoleTests;

class Program
{
    private static bool __TimerThreadCanWork = true;

    private static async Task<int> TestTask()
    {
        await Task.Yield();
        return 13;
    }

    public static void Main(string[] args)
    {
        var current_context = SynchronizationContext.Current;

        var task = TestTask();
        task.Wait();

        CriticalSectionTests.Run(); 
        return;

        var timer_thread = new Thread(() => TimerThread());
        //timer_thread.IsBackground = true;
        timer_thread.Start();

        //timer_thread.IsAlive
        //timer_thread.ThreadState == 
        timer_thread.Priority = ThreadPriority.Highest;
        timer_thread.Name = "Поток часов";

        Thread.CurrentThread.Name = "GUI";

        Console.WriteLine("Готов!");
        Console.ReadLine();

        //var threads = Process.GetCurrentProcess().Threads;

        __TimerThreadCanWork = false;
        //timer_thread.Join();
        if (timer_thread.Join(200))
        {
            Console.WriteLine("Поток часов успешно завершён");
        }
        else
        {
            Console.WriteLine("Дождаться завершения потока часов не удалось");
            //timer_thread.Interrupt();   // не безопасно
            //timer_thread.Abort();     // опасно!
        }


        Console.WriteLine("Основная программа завершена");
    }

    private static void TimerThread()
    {
        while (__TimerThreadCanWork)
        {
            Console.Title = DateTime.Now.ToString("HH:mm:ss.fff");
            Thread.Sleep(100);
            //Thread.SpinWait(1000);
        }

        Console.WriteLine("Поток обновления часов завершён");
    }
}