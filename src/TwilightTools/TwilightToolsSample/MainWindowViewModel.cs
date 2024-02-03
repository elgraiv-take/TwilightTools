using System;
using System.Windows.Input;
using Elgraiv.TwilightTools.Mvvm;
using Elgraiv.TwilightTools.Mvvm.Dialog;
using Elgraiv.TwilightTools.SnowDock;
using Elgraiv.TwilightToolsSample;

namespace Elgraiv.TwilightToolsSample
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

            DockManager.RegisterPanelViewModel(new SampleTabViewModel("TabA", "TabA") { PreferedPath = new LayoutPath() });
            DockManager.RegisterPanelViewModel(new SampleTabViewModel("TabB", "TabB") { PreferedPath = new LayoutPath([1, 0]) });
            DockManager.RegisterPanelViewModel(new SampleTabViewModel("TabC", "TabC") { PreferedPath = new LayoutPath([1, 0]) });
            DockManager.RegisterPanelViewModel(new SampleTabViewModel("TabD", "TabD") { PreferedPath = new LayoutPath([1, 1]) });
            DockManager.RegisterPanelViewModel(new SampleTabViewModel("TabE", "TabE") { PreferedPath = new LayoutPath([2]) });

            DockManager.BuildLayout();

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
