using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Elgraiv.TwilightTools.SnowDock.Controls;

internal class DockingAdorner : Adorner,IDisposable
{
    private AdornerLayer _layer;
    private DockingAdornerPanel _panel;
    private Size _size;
    public DockingAdorner(BranchPanel adornedElement, DockingAdornerPanel panel,AdornerLayer layer) : base(adornedElement)
    {
        _layer = layer;
        _layer.Add(this);
        _panel = panel;
        _size = new Size(_panel.Width, _panel.Height);
        AddVisualChild(_panel);
        AddLogicalChild(_panel);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        _panel.Arrange(new Rect(_size));
        return _size;
    }

    protected override Size MeasureOverride(Size constraint)
    {

        _panel.Measure(_size);
        return _size;
    }

    protected override Visual GetVisualChild(int index)
    {
        return _panel;
    }
    protected override IEnumerator LogicalChildren => new[] { _panel }.GetEnumerator();

    protected override int VisualChildrenCount => 1;

    public void Dispose()
    {
        _layer.Remove(this);
    }
}
