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
public class DockTabPanelItem : TabItem
{
    static DockTabPanelItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DockTabPanelItem), new FrameworkPropertyMetadata(typeof(DockTabPanelItem)));
    }

    private DockTabPanel _placed;
    public DockTabPanelItem(DockTabPanel parent)
    {
        _placed = parent;
    }

    private Point _startPoint;
    private bool _isDraging = false; 
    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);

        if (_placed.Model.Root.ContentCount <= 1)
        {
            return;
        }

        if (!CaptureMouse())
        {
            return;
        }
        _isDraging = true;
        _startPoint = PointToScreen(e.GetPosition(this));

    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (_isDraging)
        {
            var current = PointToScreen(e.GetPosition(this));
            var delta = current - _startPoint;
            if (Math.Abs(delta.Y) > 50)
            {
                PopNewWindow(current);
            }
        }

    }

    private void PopNewWindow(Point position)
    {
        ReleaseMouseCapture();
        _isDraging = false;

        if(DataContext is not LayoutContent content)
        {
            return;
        }

        var dpi=VisualTreeHelper.GetDpi(this);
        var targetRect = new Rect(position.X / dpi.DpiScaleX, position.Y / dpi.DpiScaleY, _placed.ActualWidth, _placed.ActualHeight);
        _placed.Root.RequestFloatingWindow(targetRect, content);


    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        if (_isDraging)
        {
            ReleaseMouseCapture();
            _isDraging = false;
        }
    }
}
