using System;
using System.Windows.Input;
using Elgraiv.TwilightTools.Mvvm;
using Elgraiv.TwilightTools.Mvvm.Dialog;
using Elgraiv.TwilightTools.SnowDock;

namespace TwilightToolsSample
{
    public class MainWindowViewModel : BindableBase, INotifyMessageSent
    {
        public event EventHandler<DialogMessageEventArgs>? NotifyDialogMessageSent;

        public ICommand ShowDialogCommand { get; }

        public DockManager DockManager { get; }

        public MainWindowViewModel()
        {
            ShowDialogCommand = new DelegateCommand(ShowDialog);

            DockManager = new DockManager();

        }

        private void ShowDialog()
        {
            var dialogViewModel = new MessageDialogViewModel()
            {
                Title = "SampleDialog",
                Message = "SampleMessage\nDummyDumy",
                IsOkOnly = true,
            };
            NotifyDialogMessageSent?.Invoke(this, new DialogMessageEventArgs(dialogViewModel));
        }
    }
}
