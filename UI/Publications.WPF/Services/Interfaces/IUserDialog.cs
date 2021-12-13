namespace Publications.WPF.Services.Interfaces
{
    public interface IUserDialog
    {
        void Message(string Message, string Title = "Сообщение");

        bool Question(string Message, string Title = "Вопрос");

        void ShowSettings();
    }
}
