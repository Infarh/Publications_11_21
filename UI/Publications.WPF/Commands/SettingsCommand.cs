using Publications.WPF.Commands.Base;
using Publications.WPF.Views.Windows;

namespace Publications.WPF.Commands
{
    public class SettingsCommand : Command
    {
        private readonly SettingsWindow _Window;

        public SettingsCommand(SettingsWindow Window) { _Window = Window; }

        public override void Execute(object? parameter)
        {
            _Window.ShowDialog();
        }
    }
}
