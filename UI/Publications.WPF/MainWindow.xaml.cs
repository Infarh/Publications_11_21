using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Publications.WPF
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private CancellationTokenSource _OperationCancellation;

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var thread_id_start = Thread.CurrentThread.ManagedThreadId;

            // Thread id == 1 - GUI
            // работа с интерфейсом

            Title = "Start Action";

            IProgress<double> progress = new Progress<double>(p => OperationProgress.Value = p * 100);

            await Task.Yield().ConfigureAwait(false);

            var thread_id_work = Thread.CurrentThread.ManagedThreadId;

            const int iterations_count = 100;
            for(var i = 0; i < iterations_count; i++)
            {
                await Task.Delay(100);
                progress.Report((double)i / iterations_count);
            }

           // Title = "Work Action";


            // Thread из ThreadPool - id != 1

            // сложные операции - на пример чтение файла

            await Dispatcher;

            OperationProgress.Value = 0;

            var thread_id_end = Thread.CurrentThread.ManagedThreadId;

            Title = "Action completed";


            // Thread id == 1 - GUI
            // работа с интерфейсом

        }

        private async void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            //ResultTextBlock.Text = await Task.Run(() => LongOperation(50));

            //var task = Task.Run(() => LongOperation(50));

            //var result = await Task.Run(() => LongOperation(50));

            ((Button)sender).IsEnabled = false;

            var cts = new CancellationTokenSource(/*5000*/);
            _OperationCancellation = cts;

            var progress = new Progress<double>(p => OperationProgress.Value = p * 100);

            try
            {
                var result = await LongOperationAsync(50, Progress: progress, Cancel: cts.Token);

                ResultTextBlock.Text = result;
            }
            catch(OperationCanceledException)
            {
                ResultTextBlock.Text = "Операция отменена";
            }

            ((IProgress<double>)progress).Report(0);

            ((Button)sender).IsEnabled = true;
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            _OperationCancellation?.Cancel();
        }

        private string LongOperation(int Timeout, int Count = 100)
        {
            for(var i = 0; i < Count; i++)
            {
                Thread.Sleep(Timeout);
                Debug.WriteLine("Итерация обработки данных {0}", i);
            }

            return DateTime.Now.ToLongTimeString();
        }

        private async Task<string> LongOperationAsync(
            int Timeout, int Count = 100,
            IProgress<double>? Progress = default,
            IProgress<string>? MessageInfo = default,
            CancellationToken Cancel = default)
        {
            MessageInfo?.Report("Метод запущен");
            Cancel.ThrowIfCancellationRequested();

            Debug.WriteLine("Запущена задача в потоке {0}", Thread.CurrentThread.ManagedThreadId);

            //await Task.CompletedTask.ConfigureAwait(false);

            for (var i = 0; i < Count; i++)
            {
                if (Cancel.IsCancellationRequested)
                {
                    // почистить ресурсы в случае отмены
                    Cancel.ThrowIfCancellationRequested();
                    //throw new OperationCanceledException(Cancel);
                }

                //Thread.Sleep(Timeout);
                await Task.Delay(Timeout, Cancel).ConfigureAwait(false);
                Debug.WriteLine("Итерация обработки данных {0} в потоке {1}", 
                    i, Thread.CurrentThread.ManagedThreadId);

                Progress?.Report((double)i / Count);
                MessageInfo?.Report($"Метод работает {i}");

            }

            Cancel.ThrowIfCancellationRequested();
            MessageInfo?.Report("Метод завершён");

            return DateTime.Now.ToLongTimeString();
        }
    }
}
