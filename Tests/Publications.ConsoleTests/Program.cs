namespace Publications.ConsoleTests;

class Program
{
    public static async Task Main(string[] args)
    {
        //    //ThreadPool.SetMaxThreads(10, 10);

        var messages = Enumerable.Range(1, 100).Select(i => $"Message {i}").ToArray();

        //    foreach (var msg in messages)
        //    {
        //        ThreadPool.QueueUserWorkItem(parameter =>
        //        {
        //            Thread.Sleep(1000);
        //            Console.WriteLine("Message processed: {0}", msg);
        //        });
        //    }

        //    Console.ReadLine();

        //var task = new Task(() => TaskAction("Hello World!"));
        //task.Start();
        //task.Wait();


        var task = Task.Run(() => TaskAction("Hello World!"));
        var wait_task = task.ContinueWith(t => Console.WriteLine("Задача id:{0} завершилась с результатом", task.Id, task.Result));
        //var result = task.Result;
        //var error = task.Exception;

        var result = await task.ConfigureAwait(true);

        await wait_task.ConfigureAwait(true);

        //TaskActionAsync("123").Wait();
    }

    private static int TaskAction(string Message)
    {
        for (var i = 0; i < 10; i++)
        {
            Console.WriteLine("Message: {0}", Message);

            Thread.Sleep(250);
        }

        return Message.Length;
    }

    private static async Task<int> TaskActionAsync(string Message)
    {
        await Task.Yield();

        for (var i = 0; i < 10; i++)
        {
            //await Task.Yield();


            Console.WriteLine("Message: {0}", Message);

            //Thread.Sleep(250);
            await Task.Delay(250).ConfigureAwait(false);
        }

        return Message.Length;
    }
}