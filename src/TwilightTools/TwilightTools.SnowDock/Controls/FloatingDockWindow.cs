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
public class FloatingDockWindow : Window
{
    private const string ElementNameRootBranch = "PART_RootBranch";
    static FloatingDockWindow()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(FloatingDockWindow), new FrameworkPropertyMetadata(typeof(FloatingDockWindow)));
    }

    private LayoutSystem _layoutSystem;
    internal FloatingRoot WindowRootModel { get; }
    private DockRootPanel _rootPanel;
    internal FloatingDockWindow(LayoutSystem layoutSystem, FloatingRoot windowRoot, DockRootPanel rootPanel)
    {
        _layoutSystem = layoutSystem;
        WindowRootModel = windowRoot;
        _rootPanel = rootPanel;

    }


    private BranchPanel? _rootBranch;
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _rootBranch = GetTemplateChild(ElementNameRootBranch) as BranchPanel;

        ResetLayout();
    }

    private void ResetLayout()
    {
        if (_rootBranch is null)
        {
            return;
        }

        _rootBranch.SetLayout(WindowRootModel.Root.Layout, _rootPanel);
    }

    private bool _moving = false;
    private Point _start;

    internal void SetMoveTarget()
    {
        _moving = CaptureMouse();
        _start = PointToScreen(Mouse.GetPosition(this));
    }


    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (_moving)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                EndDrag();
                return;
            }
            var current = PointToScreen(e.GetPosition(this));
            var dpi = VisualTreeHelper.GetDpi(this);
            Left = current.X / dpi.DpiScaleX;
            Top = current.Y / dpi.DpiScaleY;

            _rootPanel.HitCheck(this, current);
        }
    }

    protected override void OnPreviewMouseMove(MouseEventArgs e)
    {

    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        EndDrag();
    }

    private void EndDrag()
    {
        ReleaseMouseCapture();
        _moving = false;
        _rootPanel.EndHitCheck(WindowRootModel);
    }

    protected override void OnClosed(EventArgs e)
    {
        WindowRootModel.CloseAll();
        base.OnClosed(e);
    }
}
