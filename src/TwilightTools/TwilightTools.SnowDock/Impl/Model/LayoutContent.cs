using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.TwilightTools.SnowDock.Impl.Model
{
    internal class LayoutContent
    {
        public string Id { get; }
        public LayoutContent(string id)
        {
            Id = id;
        }
        public LayoutPath? ExpectedPath { get; set; }

        public TabLayout? Tab { get; set; }
    }
}
