using Microsoft.Extensions.DependencyInjection;
using Publications.WPF.ViewModels;

namespace Publications.WPF
{
    public class ServiceLocator
    {
        public MainWindowViewModel MainModel => App.Services.GetRequiredService<MainWindowViewModel>();
    }
}
