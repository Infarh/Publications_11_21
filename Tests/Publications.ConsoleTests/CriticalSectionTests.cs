namespace Publications.ConsoleTests;

internal static class CriticalSectionTests
{
    public static void Run()
    {
        var list = new List<string>();

        var threads = new Thread[10];

        for (var i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(() =>
            {
                //var thread_id = Thread.CurrentThread.ManagedThreadId;
                var thread_id = Environment.CurrentManagedThreadId;
                for (var j = 0; j < 100; j++)
                {
                    //lock (list)
                    //    list.Add($"Data value {j} from thread id: {thread_id}");
                    //Thread.Sleep(10);

                    lock (threads)
                    {
                        Console.Write("Data ");
                        Console.Write(j);
                        Console.Write(" from thread id: ");
                        Console.WriteLine(thread_id);
                    }
                }
            });
            threads[i].Start();
        }

        Console.WriteLine("Потоки запущены");
        Console.ReadLine();
    }
}