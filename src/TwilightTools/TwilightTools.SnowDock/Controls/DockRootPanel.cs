using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
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
[TemplatePart(Name = ElementNameRootBranch, Type = typeof(BranchPanel))]
public class DockRootPanel : Control
{

    public static readonly RoutedEvent DockingOverEvent = EventManager.RegisterRoutedEvent("DockingOver", RoutingStrategy.Bubble, typeof(DockingEventHandler), typeof(DockRootPanel));
    public static readonly RoutedEvent DockingTargetOverEvent = EventManager.RegisterRoutedEvent("DockingTargetOver", RoutingStrategy.Bubble, typeof(DockingTargetEventHandler), typeof(DockRootPanel));

    private const string ElementNameRootBranch = "PART_RootBranch";
    static DockRootPanel()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DockRootPanel), new FrameworkPropertyMetadata(typeof(DockRootPanel)));
    }


    private List<FloatingDockWindow> _floatingWindows = new();

    public DockManager? DockManager
    {
        get { return (DockManager)GetValue(DockManagerProperty); }
        set { SetValue(DockManagerProperty, value); }
    }

    public static readonly DependencyProperty DockManagerProperty =
        DependencyProperty.Register(nameof(DockManager), typeof(DockManager), typeof(DockRootPanel), new PropertyMetadata(null, OnDockManagerChanged));

    private static void OnDockManagerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (DockRootPanel)d;
        self.OnDockManagerChangedImpl(e);
    }

    private void OnDockManagerChangedImpl(DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue is DockManager oldDockManager)
        {
            oldDockManager.LayoutSystem.Root.LayoutRequested -= OnMainLayoutRequeste;
            oldDockManager.LayoutSystem.FloatingWindowAdded -= OnFloatingWindowAdded;
            oldDockManager.LayoutSystem.FloatingWindowRemoved -= OnFloatingWindowRemoved;
        }
        if (e.NewValue is DockManager newDockManager)
        {
            newDockManager.LayoutSystem.Root.LayoutRequested += OnMainLayoutRequeste;
            newDockManager.LayoutSystem.FloatingWindowAdded += OnFloatingWindowAdded;
            newDockManager.LayoutSystem.FloatingWindowRemoved += OnFloatingWindowRemoved;
        }
        ResetLayout();
    }


    private void OnMainLayoutRequeste(object? sender, EventArgs e)
    {
        ResetMainLayout();
    }


    private void OnFloatingWindowAdded(object o, FloatingWindowEventArgs e)
    {
        CreateWindow(e.TargetModel);
    }

    private void OnFloatingWindowRemoved(object o, FloatingWindowEventArgs e)
    {
        var removingWindow = _floatingWindows.FirstOrDefault((window) => window.WindowRootModel == e.TargetModel);
        removingWindow?.Close();
    }
    private Dictionary<Guid, DockTabPanel> _tabs = new();

    internal DockTabPanel GetOrCreateTab(TabLayout tabLayout)
    {
        if (!_tabs.TryGetValue(tabLayout.InternalId, out var tab))
        {
            tab = new DockTabPanel(tabLayout, this);
            _tabs[tabLayout.InternalId] = tab;
        }
        return tab;
    }


    internal void RmoveTab(DockTabPanel tab)
    {
        _tabs.Remove(tab.Model.InternalId);
    }

    private void ResetLayout()
    {
        ResetMainLayout();
    }
    private void ResetMainLayout()
    {
        if (_rootBranch is null || DockManager is null)
        {
            return;
        }
        var mainLayout = DockManager.LayoutSystem.Root.Layout;
        _rootBranch.SetLayout(mainLayout, this);

    }

    private void OnRootLayoutRequested(object? sender, EventArgs e)
    {

    }

    private BranchPanel? _rootBranch;
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _rootBranch = GetTemplateChild(ElementNameRootBranch) as BranchPanel;

        ResetLayout();

    }


    /*
     * 
     * 
     *
     *
     *
     *
     */
    internal void RequestFloatingWindow(Rect rect, LayoutContent content)
    {
        if (DockManager is null)
        {
            return;
        }
        DockManager.LayoutSystem.RequestFloating(content, rect);
    }

    private void CreateWindow(FloatingRoot floating)
    {
        if (DockManager is null)
        {
            return;
        }
        Rect rect = floating.WindowRect;

        var parent = Window.GetWindow(this);
        var window = new FloatingDockWindow(DockManager.LayoutSystem, floating, this)
        {
            Left = rect.X,
            Top = rect.Y,
            Width = rect.Width,
            Height = rect.Height,
            Owner = parent,
            WindowStartupLocation = WindowStartupLocation.Manual,
        };

        _floatingWindows.Add(window);
        window.Closed += OnFloatingWindowClosed;

        window.Show();
        window.SetMoveTarget();
    }

    private void OnFloatingWindowClosed(object? sender, EventArgs e)
    {
        if (sender is FloatingDockWindow window)
        {
            window.Closed -= OnFloatingWindowClosed;
            _floatingWindows.Remove(window);
        }
    }

    /*
     *
     *
     *
     *
     *
     *
     *
     */
    private BranchPanel? _currentDockingTarget;
    private DockTabPanel? _currentDockingTab;
    private DockingTargetIcon? _targetPlace;
    internal void HitCheck(FloatingDockWindow source, Point screenCoord)
    {
        bool isHit = false;
        var rootWindow = Window.GetWindow(this);

        foreach (var window in _floatingWindows)
        {
            if (source == window)
            {
                continue;
            }
            isHit = HitCheckWindow(window, screenCoord);
            if (isHit)
            {
                break;
            }
        }
        if (!isHit)
        {
            isHit = HitCheckWindow(rootWindow, screenCoord);
        }

        if (isHit)
        {
            DockingTargetIcon? placeTarget = null;
            if (_currentDockingTarget is not null)
            {
                var layer = AdornerLayer.GetAdornerLayer(_currentDockingTarget);
                if (layer is not null)
                {
                    var point = layer.PointFromScreen(screenCoord);
                    var hitElement = layer.InputHitTest(point);
                    if (hitElement is not null)
                    {
                        var args = new DockingTargetEventArgs()
                        {
                            RoutedEvent = DockingTargetOverEvent,
                        };
                        hitElement.RaiseEvent(args);
                        placeTarget = args.TargetIcon;
                    }
                }
            }
            SetPlaceTarget(placeTarget);


        }
        else
        {
            SetPlaceTarget(null);
            ClearHit();
        }
    }

    private void SetPlaceTarget(DockingTargetIcon? placeTarget)
    {
        if (_targetPlace == placeTarget)
        {
            return;
        }
        _targetPlace?.ClearOver();
        _targetPlace = placeTarget;
    }

    private bool HitCheckWindow(Window window, Point screenCoord)
    {

        var point = window.PointFromScreen(screenCoord);
        var hitElement = window.InputHitTest(point);
        if (hitElement is not null)
        {
            var args = new DockingEventArgs()
            {
                RoutedEvent = DockingOverEvent,
            };
            hitElement.RaiseEvent(args);
            if (args.TargetPanel is not null)
            {
                SetCurrentDockingTarget(args.TargetPanel);
                _currentDockingTab = args.TargetTab;
            }
            return true;
        }
        return false;
    }

    internal void EndHitCheck(FloatingRoot docking)
    {
        if (_currentDockingTarget is not null && _targetPlace is not null)
        {
            PlaceTab(docking);
        }
        ClearHit();
    }

    private void PlaceTab(FloatingRoot docking)
    {
        if (_targetPlace is null || DockManager is null)
        {
            return;
        }
        var content = docking.Root.Contents.FirstOrDefault();
        if (content is null)
        {
            return;//引っかからないはず
        }

        switch (_targetPlace.TargetPlace)
        {
            case DockingTargetPlace.Panel:
                {
                    if (_currentDockingTab is not null)
                    {
                        var targetPath = _currentDockingTab.Model.ComputeCurrentPath();

                        DockManager.LayoutSystem.RequestDock(targetPath, content);
                    }
                }
                break;
            case DockingTargetPlace.PanelLeft:
                break;
            case DockingTargetPlace.PanelTop:
                break;
            case DockingTargetPlace.PanelRight:
                break;
            case DockingTargetPlace.PanelBottom:
                break;
            case DockingTargetPlace.RootLeft:
                break;
            case DockingTargetPlace.RootTop:
                break;
            case DockingTargetPlace.RootRight:
                break;
            case DockingTargetPlace.RootBottom:
                break;
            default:
                break;
        }
    }

    private void ClearHit()
    {

        _currentDockingTarget?.ClearAdorner();
        _currentDockingTarget = null;
        _currentDockingTab = null;
        _targetPlace?.ClearOver();
        _targetPlace = null;
    }

    private void SetCurrentDockingTarget(BranchPanel branchPanel)
    {
        if (_currentDockingTarget == branchPanel)
        {
            return;
        }
        _currentDockingTarget?.ClearAdorner();
        _currentDockingTarget = branchPanel;
    }
}
