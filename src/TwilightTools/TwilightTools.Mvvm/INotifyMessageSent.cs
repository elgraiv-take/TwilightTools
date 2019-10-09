using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.TwilightTools.Mvvm
{
    public class DialogMessageEventArgs : EventArgs
    {
        public IDialogViewModel ViewModel { get; }
        public DialogMessageEventArgs(IDialogViewModel viewModel)
        {
            ViewModel = viewModel;
        }
    }

    public interface INotifyMessageSent
    {
        event EventHandler<DialogMessageEventArgs> NotifyDialogMessageSent;
    }
}
