using Elgraiv.TwilightTools;
using Elgraiv.TwilightTools.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TwilightToolsSample
{
    public class MainWindowViewModel : BindableBase, INotifyMessageSent
    {
        public event EventHandler<DialogMessageEventArgs> NotifyDialogMessageSent;

        public ICommand ShowDialogCommand { get; }

        public MainWindowViewModel()
        {
            ShowDialogCommand = new DelegateCommand(ShowDialog);
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
