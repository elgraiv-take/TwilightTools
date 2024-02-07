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

namespace Elgraiv.TwilightTools.SnowDock.Controls;
public class PanelSplitter : Control
{
    private class PanelSplitterAdorner : Adorner, IDisposable
    {
        private AdornerLayer _layer;
        public PanelSplitterAdorner(UIElement adornedElement, AdornerLayer layer) : base(adornedElement)
        {
            _layer = layer;
            _layer.Add(this);
        }

        public void Dispose()
        {
            _layer.Remove(this);
        }

        public Point Offset { get; set; }
        public double NewPosition { get; set; }
        public double RangeMin { get; set; }
        public double RangeMax { get; set; }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Rect originRect = new Rect(Offset, AdornedElement.RenderSize);

            SolidColorBrush renderBrush = new SolidColorBrush(Colors.Green);

            // Draw a circle at each corner.
            drawingContext.DrawRectangle(renderBrush, null, originRect);
        }
    }

    static PanelSplitter()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(PanelSplitter), new FrameworkPropertyMetadata(typeof(PanelSplitter)));
    }

    public double PositionRate { get; set; }

    public Orientation Orientation
    {
        get { return (Orientation)GetValue(OrientationProperty); }
        private set { SetValue(OrientationPropertyKey, value); }
    }

    public static readonly DependencyPropertyKey OrientationPropertyKey =
        DependencyProperty.RegisterReadOnly(nameof(Orientation), typeof(Orientation), typeof(PanelSplitter), new PropertyMetadata(Orientation.Horizontal));

    public static readonly DependencyProperty OrientationProperty = OrientationPropertyKey.DependencyProperty;


    private BranchPanel _parent;

    public PanelSplitter(Orientation orientation, BranchPanel parent)
    {
        Orientation = orientation;
        _parent = parent;
    }

    private PanelSplitterAdorner? _adorner;
    private Point _startPosition;

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        if (!CaptureMouse())
        {
            return;
        }
        var decorator = AdornerLayer.GetAdornerLayer(this);
        if (decorator is not null)
        {
            _startPosition = e.GetPosition(this);
            var (min, max) = _parent.SplitterRange(this);
            _adorner?.Dispose();
            _adorner = new(this, decorator)
            {
                NewPosition = PositionRate,
                RangeMin = min,
                RangeMax = max,
            };
            e.MouseDevice.OverrideCursor = Orientation == Orientation.Horizontal ? Cursors.SizeWE : Cursors.SizeNS;
            e.Handled = true;
        }

    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (_adorner is not null)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                e.MouseDevice.OverrideCursor = null;
                _adorner.Dispose();
                _adorner = null;
                return;
            }
            var coerce = (double delta, double size) =>
            {
                var a = PositionRate + (delta / size);
                if (a < _adorner.RangeMin)
                {
                    a = _adorner.RangeMin;
                    delta = (a - PositionRate) * size;
                }
                else if (a > _adorner.RangeMax)
                {
                    a = _adorner.RangeMax;
                    delta = (a - PositionRate) * size;
                }
                _adorner.NewPosition = a;
                return delta;
            };
            var current = e.GetPosition(this);
            if (Orientation == Orientation.Horizontal)
            {
                var delta = current.X - _startPosition.X;
                delta = coerce(delta, _parent.ActualWidth);
                _adorner.Offset = new Point(delta, 0.0);
            }
            else
            {
                var delta = current.Y - _startPosition.Y;
                delta = coerce(delta, _parent.ActualHeight);
                _adorner.Offset = new Point(0.0, delta);

            }
            _adorner.InvalidateVisual();
            e.Handled = true;
        }
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        if (_adorner is not null)
        {
            PositionRate = _adorner.NewPosition;
            _parent.InvalidateMeasure();
            _adorner.Dispose();
            _adorner = null;
        }
        e.MouseDevice.OverrideCursor = null;
        ReleaseMouseCapture();
    }




}
