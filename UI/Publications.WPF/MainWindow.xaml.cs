using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Publications.WPF
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var calculation_thread = new Thread(() => UpdateData());
            calculation_thread.Start();
        }

        private /*async*/ void UpdateData()
        {
            var result = CalculateData();
            //ResultTextBlock.Text = result;
            //ResultTextBlock.Dispatcher.Invoke(() =>
            //{
            //    ResultTextBlock.Text = result;
            //});
            //Application.Current.Dispatcher.BeginInvoke(() =>
            //{
            //    ResultTextBlock.Text = result;
            //});
            var operation = Application.Current.Dispatcher.InvokeAsync(() =>
            {
                ResultTextBlock.Text = result;
            }, System.Windows.Threading.DispatcherPriority.Background);

            //await operation.Task;
        }

        private static string CalculateData()
        {
            Thread.Sleep(2000);
            return "Результат вычислений " + DateTime.Now.ToString();
        }
    }
}
