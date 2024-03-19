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

            DockManager.RegisterPanelViewModel(new SampleTabViewModel("TabA", "TabA") { PreferedPath = new LayoutPath([3, 1, 1, 0]) });
            DockManager.RegisterPanelViewModel(new SampleTabViewModel("TabB", "TabB") { PreferedPath = new LayoutPath([3, 1, 1, 1]) });
            DockManager.RegisterPanelViewModel(new SampleTabViewModel("TabC", "TabC") { PreferedPath = new LayoutPath([3, 1, 2, 2]) });
            DockManager.RegisterPanelViewModel(new SampleTabViewModel("TabD", "TabD") { PreferedPath = new LayoutPath([0]) });
            DockManager.RegisterPanelViewModel(new SampleTabViewModel("TabE", "TabE") { PreferedPath = new LayoutPath([0], 0) });
            DockManager.RegisterPanelViewModel(new SampleTabViewModel("TabF", "TabF") { PreferedPath = new LayoutPath([1]) });
            DockManager.RegisterPanelViewModel(new SampleTabViewModel("TabG", "TabG") { PreferedPath = new LayoutPath([1, 0]) });
            DockManager.RegisterPanelViewModel(new SampleTabViewModel("TabH", "TabH") { PreferedPath = new LayoutPath([1, 1]) });
            DockManager.RegisterPanelViewModel(new SampleTabViewModel("TabI", "TabI") { PreferedPath = new LayoutPath([1, 2, 0]) });
            DockManager.RegisterPanelViewModel(new SampleTabViewModel("TabJ", "TabJ") { PreferedPath = new LayoutPath([1, 2, 0]) });
            DockManager.RegisterPanelViewModel(new SampleTabViewModel("TabK", "TabK") { PreferedPath = new LayoutPath([1, 2]) });
            DockManager.RegisterPanelViewModel(new SampleTabViewModel("TabL", "TabL") { PreferedPath = new LayoutPath([1, 2, 1]) });
            DockManager.RegisterPanelViewModel(new SampleTabViewModel("TabM", "TabM") { PreferedPath = new LayoutPath([2]) });


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
