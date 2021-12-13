using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Publications.WPF.Services.Interfaces;
using Publications.WPF.ViewModels;
using Publications.WPF.Views.Windows;

namespace Publications.WPF.Services
{
    public class UserDialog : IUserDialog
    {
        private readonly IServiceProvider _Services;

        public UserDialog(IServiceProvider services) => _Services = services;

        public void Message(string Message, string Title = "Сообщение")
        {
            MessageBox.Show(Message, Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public bool Question(string Message, string Title = "Вопрос")
        {
            var result = MessageBox.Show(Message, Title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

        public void ShowSettings()
        {
            var settings_view_model = _Services.GetRequiredService<SettingsWindowViewModel>();

            var settings_window = new SettingsWindow
            {
                DataContext = settings_view_model,
                Owner = Application.Current.MainWindow,
            };

            settings_window.ShowDialog();


        }
    }
}
