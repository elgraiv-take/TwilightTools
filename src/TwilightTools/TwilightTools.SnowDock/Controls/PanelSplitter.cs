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
    static PanelSplitter()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(PanelSplitter), new FrameworkPropertyMetadata(typeof(PanelSplitter)));
    }

    public double PositionRate { get; set; }

    public PanelSplitter()
    {
    }
}
