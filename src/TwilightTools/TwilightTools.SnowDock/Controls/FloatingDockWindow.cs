using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
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
[TemplatePart(Name = ElementWindowHeaderBranch, Type = typeof(FrameworkElement))]
public class FloatingDockWindow : Window
{
    private const string ElementNameRootBranch = "PART_RootBranch";
    private const string ElementWindowHeaderBranch = "PART_WindowHeader";
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
    private FrameworkElement? _header;
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _rootBranch = GetTemplateChild(ElementNameRootBranch) as BranchPanel;
        _header = GetTemplateChild(ElementWindowHeaderBranch) as FrameworkElement;

        if(_header is not null)
        {
            _header.MouseLeftButtonDown += OnHeaderMouseLeftButtonDown;
        }

        ResetLayout();
    }

    private void OnHeaderMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {

        var start = PointToScreen(Mouse.GetPosition(this));
        var dpi = VisualTreeHelper.GetDpi(this);
        var offset = new Point(Left - start.X / dpi.DpiScaleX, Top - start.Y / dpi.DpiScaleY);
        SetMoveTarget(offset);
        e.Handled = true;
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
    private Point _offset;

    internal void SetMoveTarget(Point offset)
    {
        _moving = CaptureMouse();

        _offset = offset;
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
            Left = current.X / dpi.DpiScaleX + _offset.X;
            Top = current.Y / dpi.DpiScaleY + _offset.Y;

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
