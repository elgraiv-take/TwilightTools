using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Elgraiv.TwilightTools.SnowDock.Impl;
using Elgraiv.TwilightTools.SnowDock.Impl.Model;

namespace Elgraiv.TwilightTools.SnowDock.Controls;

public class DockTabPanel : TabControl
{

    static DockTabPanel()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DockTabPanel), new FrameworkPropertyMetadata(typeof(DockTabPanel)));

        EventManager.RegisterClassHandler(typeof(DockTabPanel), DockRootPanel.DockingOverEvent, new DockingEventHandler(OnDockingOverThunk), false);

    }

    private static void OnDockingOverThunk(object? sender, DockingEventArgs e)
    {
        if (e.Handled)
        {
            return;
        }
        if (sender is DockTabPanel self)
        {
            self.OnDockingOver(e);
        }
    }

    private void OnDockingOver(DockingEventArgs e)
    {
        if (RootBranch is null)
        {
            return;
        }
        RootBranch.SetAdorner(this);
        e.TargetPanel = RootBranch;
        e.TargetTab = this;
        e.Handled = true;
    }

    internal TabLayout Model { get; }
    internal DockRootPanel Root { get; }

    internal BranchPanel? RootBranch { get; set; }
    internal DockTabPanel(TabLayout model, DockRootPanel root)
    {
        Model = model;
        Root = root;

        ItemsSource = Model.Contents;

        model.TabDeleting += OnTabModelDeleting;
    }

    private void OnTabModelDeleting(object? sender, EventArgs e)
    {
        Model.TabDeleting -= OnTabModelDeleting;
        Root.RmoveTab(this);
    }
    protected override DependencyObject GetContainerForItemOverride() => new DockTabPanelItem(this);

}
