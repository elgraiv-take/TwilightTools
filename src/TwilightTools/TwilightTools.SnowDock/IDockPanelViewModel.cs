using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.TwilightTools.SnowDock;
public interface IDockPanelViewModel
{
    public string Label { get; }

    public bool IsVisible { get; set; }

    public string ContentId { get; }
    public LayoutPath? PreferedPath { get; }
}
