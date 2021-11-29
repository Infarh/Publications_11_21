namespace Publications.ConsoleTests;

class Program
{
    public static void Main(string[] args)
    {
        var list = new List<string>();

        var manual_event = new ManualResetEvent(false);
        var auto_event = new AutoResetEvent(false);

        //Mutex mutex = new Mutex(true, "My singleton program", out var is_first);
        //mutex.ReleaseMutex();

        Semaphore semaphore = new Semaphore(3, 3);


        var threads = new Thread[10];
        for (var i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(() =>
            {
                var thread_id = Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine("Поток {0} создан и ждёт разрешения", thread_id);

                semaphore.WaitOne();

                //auto_event.WaitOne();
                Console.WriteLine("Поток {0} запущен", thread_id);

                for (var j = 0; j < 10; j++)
                {
                    list.Add($"Thread value {j} ftom thread id: {thread_id}");
                    Console.WriteLine($"Thread value {j} ftom thread id: {thread_id}");
                    Thread.Sleep(250);
                }

                Console.WriteLine("Поток {0} завершён", thread_id);

                semaphore.Release();
            });

            threads[i].Start();
        }

        Console.WriteLine("Все потоки запущены и готовы к работе");
        Console.ReadLine();

        auto_event.Set();
        Console.WriteLine("Потокам разрешено выполнить работу");

        Console.ReadLine();
        auto_event.Set();

        Console.ReadLine();
    }
}