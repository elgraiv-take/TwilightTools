using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elgraiv.TwilightTools.SnowDock;

namespace Elgraiv.TwilightToolsSample;
internal class SampleTabViewModel(string label, string id) : IDockPanelViewModel
{
    public string Header { get; init; } = label;

    public bool IsVisible { get; set; }

    public string ContentId { get; init; } = id;

    public LayoutPath? PreferedPath { get; init; }

    public override string ToString() => Header;
}
