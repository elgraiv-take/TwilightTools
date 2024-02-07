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
public class DockingAdornerPanel : Control
{
    static DockingAdornerPanel()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DockingAdornerPanel), new FrameworkPropertyMetadata(typeof(DockingAdornerPanel)));
    }

    DockTabPanel _tab;
    public DockingAdornerPanel(BranchPanel root, DockTabPanel tab)
    {
        _tab = tab;
    }


    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        var internalPanel = GetTemplateChild("PART_InternalBorder") as FrameworkElement;
        if (internalPanel is not null)
        {

            internalPanel.Width = _tab.ActualWidth;
            internalPanel.Height = _tab.ActualHeight;
            var offset = _tab.TranslatePoint(new Point(), this);
            internalPanel.Margin = new Thickness(offset.X, offset.Y, 0.0, 0.0);
        }

    }
}
