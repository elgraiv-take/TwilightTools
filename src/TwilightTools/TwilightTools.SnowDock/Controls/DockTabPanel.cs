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

public class DockTabPanel : TabControl
{
    static DockTabPanel()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DockTabPanel), new FrameworkPropertyMetadata(typeof(DockTabPanel)));
    }

    internal TabLayout Model { get; }

    internal DockTabPanel(TabLayout model)
    {
        Model = model;


        ItemsSource = Model.Contents;
    }

    protected override DependencyObject GetContainerForItemOverride() => new DockTabPanelItem();
}
