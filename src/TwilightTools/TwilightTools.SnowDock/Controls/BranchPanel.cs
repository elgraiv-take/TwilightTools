using System;
using System.Collections;
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
public class BranchPanel : FrameworkElement
{
    static BranchPanel()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(BranchPanel), new FrameworkPropertyMetadata(typeof(BranchPanel)));
    }

    private UIElementCollection _uieCollection;

    public Orientation Orientation { get; }
    public double SplitterWidth { get; set; } = 5.0;

    private record ChildElement(FrameworkElement Panel, PanelSplitter Splitter)
    {
        private Rect _bounds;
        public ref Rect Bounds => ref _bounds;
    }

    private List<ChildElement> _childrenPanels = new();
    //private List<PanelSplitter> _splitters = new();

    public BranchPanel() : this(LayoutOrientation.Horisontal)
    {

    }
    internal BranchPanel(LayoutOrientation orientation)
    {
        Orientation = orientation switch
        {
            LayoutOrientation.Horisontal => Orientation.Horizontal,
            LayoutOrientation.Vertical => Orientation.Vertical,
            _ => Orientation.Horizontal,
        };
        _uieCollection = new UIElementCollection(this, this);
        /*
        AddChildPanel(new Border() { Background = Brushes.AliceBlue });
        AddChildPanel(new Border() { Background = Brushes.AliceBlue });
        */
    }
    protected override void OnRender(DrawingContext dc)
    {
        /*
        Brush background = Brushes.Red;
        Size renderSize = RenderSize;
        dc.DrawRectangle(background,
                         null,
                         new Rect(0.0, 0.0, renderSize.Width, renderSize.Height));
      */
    }

    internal void SetLayout(IIntermediateLayout layout, DockRootPanel root)
    {
        foreach(var child in layout.Children)
        {
            switch (child)
            {
                case IIntermediateLayout imd:
                    {
                        var panel = new BranchPanel(Orientation == Orientation.Horizontal ? LayoutOrientation.Vertical : LayoutOrientation.Horisontal);
                        panel.SetLayout(imd, root);
                        AddChildPanel(panel);
                    }
                    break;
                case TabLayout tab:
                    AddChildPanel(root.GetOrCreateTab(tab));
                    break;
                default:
                    break;
                    
            }
        }
    }

    internal void AddChildPanel(FrameworkElement panel)
    {
        _uieCollection.Insert(0,panel);
        var splitter = new PanelSplitter();
        _uieCollection.Insert(_uieCollection.Count, splitter);
        _childrenPanels.Add(new(panel, splitter));

        var step = 1.0 / _childrenPanels.Count;

        var offset = step;
        foreach (var (_,child) in _childrenPanels)
        {
            child.PositionRate = offset;
            offset += step;
        }

        InvalidateMeasure();
    }

    protected override int VisualChildrenCount => _uieCollection.Count;
    protected override Visual GetVisualChild(int index)
    {
        return _uieCollection[index];
    }
    protected override IEnumerator LogicalChildren => _uieCollection.GetEnumerator();

    protected override Size ArrangeOverride(Size finalSize)
    {
        /*
        foreach(UIElement child in _uieCollection)
        {
            var rect = new Rect(2, 2, finalSize.Width - 4, finalSize.Height - 4);
            //var rect = new Rect(finalSize);
            child.Arrange(rect);
        }
        */

        foreach (var item in _childrenPanels)
        {
            item.Panel.Arrange(item.Bounds);

        }
        var splitterSize = Orientation == Orientation.Horizontal ?
            new Size(SplitterWidth, finalSize.Height) : new Size(finalSize.Width, SplitterWidth);
        foreach (var item in _childrenPanels.SkipLast(1))
        {
            var x = item.Bounds.X;
            var y = item.Bounds.Y;
            if(Orientation == Orientation.Horizontal)
            {
                x += item.Bounds.Width;
            }
            else
            {
                y += item.Bounds.Height;
            }
            item.Splitter.Arrange(new Rect(new Point(x,y), splitterSize));

        }
        return finalSize;
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        if (!(_childrenPanels.Count > 0))
        {
            return availableSize;
        }

        var size = availableSize;

        var currentOffset = 0.0;
        var splitterSize = Orientation == Orientation.Horizontal ?
            new Size(SplitterWidth, size.Height) : new Size(size.Width, SplitterWidth);
        foreach (var item in _childrenPanels.SkipLast(1))
        {
            ComputeChildRect(availableSize, ref item.Bounds, currentOffset, item.Splitter.PositionRate);
            item.Panel.Measure(item.Bounds.Size);
            currentOffset = item.Splitter.PositionRate;
            item.Splitter.Measure(splitterSize);
        }

        var last = _childrenPanels.Last();
        ComputeChildRect(availableSize, ref last.Bounds, currentOffset, 1.0, false);
        last.Panel.Measure(last.Bounds.Size);
        return availableSize;
    }

    private void ComputeChildRect(Size total, ref Rect bounds, double offset, double pos, bool splitter = true)
    {
        if (Orientation == Orientation.Horizontal)
        {
            bounds.X = total.Width * offset;
            bounds.Width = total.Width * (pos - offset) - (splitter ? SplitterWidth : 0.0);

            bounds.Y = 0.0;
            bounds.Height = total.Height;
        }
        else
        {

            bounds.X = 0.0;
            bounds.Width = total.Width;

            bounds.Y = total.Height * offset;
            bounds.Height = total.Height * (pos - offset) - (splitter ? SplitterWidth : 0.0);
        }
    }

}
