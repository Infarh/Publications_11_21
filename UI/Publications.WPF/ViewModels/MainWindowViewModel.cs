using System.Windows;
using System.Windows.Input;
using Publications.WPF.Commands;
using Publications.WPF.Commands.Base;
using Publications.WPF.ViewModels.Base;

namespace Publications.WPF.ViewModels;

public class MainWindowViewModel : ViewModel
{
    private string _Title = "Главное окно программы";

    public string Title { get => _Title; set => Set(ref _Title, value); }

    #region MessageText : string - Текст сообщения

    /// <summary>Текст сообщения</summary>
    private string _MessageText = "";

    /// <summary>Текст сообщения</summary>
    public string MessageText { get => _MessageText; set => Set(ref _MessageText, value); }

    #endregion

    #region ShowMessageCommand - отображение диалога с пользователем

    private Command? _ShowMessageCommand;

    public ICommand ShowMessageCommand => _ShowMessageCommand ??= new LambdaCommand(OnShowMessageCommandExecuted, CanShowMessageCommandExecute);

    private static bool CanShowMessageCommandExecute(object? parameter) => parameter switch
    {
        string msg => msg.Length > 0,
        _ => parameter is not null
    };

    private static void OnShowMessageCommandExecuted(object? parameter)
    {
        if (parameter is not { } value) return;

        var message = value as string ?? value.ToString();

        MessageBox.Show(message, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    #endregion
}