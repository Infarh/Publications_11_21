using System.Windows;
using Publications.WPF.Services.Interfaces;

namespace Publications.WPF.Services
{
    public class UserDialog : IUserDialog
    {
        public void Message(string Message, string Title = "Сообщение")
        {
            MessageBox.Show(Message, Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public bool Question(string Message, string Title = "Вопрос")
        {
            var result = MessageBox.Show(Message, Title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;

        }
    }
}
