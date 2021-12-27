using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Publications.Domain.Entities;
using Publications.Interfaces;
using Publications.Interfaces.Repositories;
using Publications.WPF.Commands;
using Publications.WPF.Commands.Base;
using Publications.WPF.Services.Interfaces;
using Publications.WPF.ViewModels.Base;

namespace Publications.WPF.ViewModels;

public class MainWindowViewModel : ViewModel
{
    private readonly IPublicationManager _PublicationManager;
    private readonly IRepository<Person> _PersonsRepository;
    private readonly IRepository<Place> _PlacesRepository;
    private readonly IUserDialog _UserDialog;

    public MainWindowViewModel(
        //IPublicationManager PublicationManager,
        IRepository<Person> PersonsRepository,
        IRepository<Place> PlacesRepository,
        IUserDialog UserDialog,
        SettingsCommand SettingsCommand)
    {
        //_PublicationManager = PublicationManager;
        _PersonsRepository = PersonsRepository;
        _PlacesRepository = PlacesRepository;
        _UserDialog = UserDialog;
        _SettingsCommand = SettingsCommand;
    }

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

    //public ICommand ShowMessageCommand => _ShowMessageCommand ??= new LambdaCommand(OnShowMessageCommandExecuted, CanShowMessageCommandExecute);
    //public ICommand ShowMessageCommand => _ShowMessageCommand ??= Command.New(OnShowMessageCommandExecuted, CanShowMessageCommandExecute);
    public ICommand ShowMessageCommand => _ShowMessageCommand ??= Command
       .Invoke(OnShowMessageCommandExecuted)
       .When(CanShowMessageCommandExecute)
       .Debug();

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

        //OnPropertyChanged();
    }

    #endregion

    #region CloseMainWindowCommand - команда закрытия главного окна

    private Command? _CloseMainWindowCommand;

    //public ICommand CloseMainWindowCommand => _CloseMainWindowCommand ??= Command.New(OnCloseMainWindowCommandExecuted);
    public ICommand CloseMainWindowCommand => _CloseMainWindowCommand ??= Command
       .Invoke(OnCloseMainWindowCommandExecuted)
       //.Invoke(p => MessageBox.Show("Было приятно с Вами работать!"))
       .When(p => p is null);

    private void OnCloseMainWindowCommandExecuted(object? p)
    {
        if (_UserDialog.Question("Реально закрыть программу?", "Закрытие программы")) 
            Application.Current.MainWindow?.Close();
    }

    #endregion

    #region Command SettingsCommand - Открыть настройки приложения

    /// <summary>Открыть настройки приложения</summary>
    private Command? _SettingsCommand;

    /// <summary>Открыть настройки приложения</summary>
    public ICommand SettingsCommand => _SettingsCommand
        ??= Command.Invoke(OnSettingsCommandExecuted);

    /// <summary>Логика выполнения - Открыть настройки приложения</summary>
    private void OnSettingsCommandExecuted(object? p)
    {
        _UserDialog.ShowSettings();
    }

    #endregion

    private ObservableCollection<string> _Items = new();

    public ICollection<string> Items => _Items;

    #region Command LoadPersonsCommand - Загрузка данных об авторах

    /// <summary>Загрузка данных об авторах</summary>
    private LambdaCommand? _LoadPersonsCommand;

    /// <summary>Загрузка данных об авторах</summary>
    public ICommand LoadPersonsCommand => _LoadPersonsCommand ??= new LambdaCommand(OnLoadPersonsCommandExecuted);

    /// <summary>Логика выполнения - Загрузка данных об авторах</summary>
    private async void OnLoadPersonsCommandExecuted(object? p)
    {
        try
        {
            var persons = await _PersonsRepository.GetAllAsync();
            Persons = persons;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Ошибка загрузки", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion

    #region Persons : IEnumerable<Person> - Авторы

    /// <summary>Авторы</summary>
    private IEnumerable<Person> _Persons;

    /// <summary>Авторы</summary>
    public IEnumerable<Person> Persons { get => _Persons; private set => Set(ref _Persons, value); }

    #endregion
}