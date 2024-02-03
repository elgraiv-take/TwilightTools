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
using Elgraiv.TwilightTools.SnowDock.Impl.Model;

namespace Elgraiv.TwilightTools.SnowDock.Controls;
[TemplatePart(Name = ElementNameRootBranch, Type = typeof(BranchPanel))]
public class DockRootPanel : Control
{
    private const string ElementNameRootBranch = "PART_RootBranch";
    static DockRootPanel()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DockRootPanel), new FrameworkPropertyMetadata(typeof(DockRootPanel)));
    }



    public DockManager? DockManager
    {
        get { return (DockManager)GetValue(DockManagerProperty); }
        set { SetValue(DockManagerProperty, value); }
    }

    public static readonly DependencyProperty DockManagerProperty =
        DependencyProperty.Register(nameof(DockManager), typeof(DockManager), typeof(DockRootPanel), new PropertyMetadata(null,OnDockManagerChanged));

    private static void OnDockManagerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (DockRootPanel)d;
        self.OnDockManagerChangedImpl(e);
    }

    private void OnDockManagerChangedImpl(DependencyPropertyChangedEventArgs e)
    {
        if(e.OldValue is DockManager oldDockManager)
        {
            oldDockManager.LayoutUpdated -= OnDockManagerLayoutUpdated;
        }
        if (e.NewValue is DockManager newDockManager)
        {
            newDockManager.LayoutUpdated += OnDockManagerLayoutUpdated;
        }
        ResetLayout();
    }

    private Dictionary<Guid, DockTabPanel> _tabs = new();

    private void OnDockManagerLayoutUpdated(object? sender, Impl.LayoutUpdatedEventArgs e)
    {

    }

    internal DockTabPanel GetOrCreateTab(TabLayout tabLayout)
    {
        if(!_tabs.TryGetValue(tabLayout.InternalId, out var tab))
        {
            tab = new DockTabPanel(tabLayout);
            _tabs[tabLayout.InternalId] = tab;
        }
        return tab;
    }

    private void ResetLayout()
    {
        if(_rootBranch is null || DockManager is null)
        {
            return;
        }

        var mainLayout = DockManager.LayoutSystem.Root.Layout;
        _rootBranch.SetLayout(mainLayout, this);
    }
    private BranchPanel? _rootBranch;
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _rootBranch = GetTemplateChild(ElementNameRootBranch) as BranchPanel;

        ResetLayout();

    }

}
