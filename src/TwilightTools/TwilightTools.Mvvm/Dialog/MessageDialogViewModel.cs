using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Elgraiv.TwilightTools.Mvvm.Dialog
{
    public class MessageDialogViewModel : BindableBase, IDialogViewModel
    {
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private bool? _dialogResult;
        public bool? DialogResult
        {
            get => _dialogResult;
            set => SetProperty(ref _dialogResult, value);
        }

        private string _acceptText = "OK";
        public string AcceptText
        {
            get => _acceptText;
            set => SetProperty(ref _acceptText, value);
        }
        private string _rejectText = "Cancel";
        public string RejectText
        {
            get => _rejectText;
            set => SetProperty(ref _rejectText, value);
        }

        private object _message;
        public object Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public ICommand AcceptCommand { get; }
        public ICommand RejectCommand { get; }

        private bool _isOkOnly = false;
        public bool IsOkOnly
        {
            get => _isOkOnly;
            set => SetProperty(ref _isOkOnly, value);
        }

        public MessageDialogViewModel()
        {
            AcceptCommand = new DelegateCommand(() => DialogResult = true);
            RejectCommand = new DelegateCommand(() => DialogResult = false);
        }

    }
}
