using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.TwilightTools.SnowDock.Impl.Model
{
    internal class LayoutContent
    {
        public string Id => ViewModel.ContentId;
        public IDockPanelViewModel ViewModel { get; }
        public LayoutContent(IDockPanelViewModel viewModel)
        {
            ViewModel = viewModel;
        }
        public LayoutPath? ExpectedPath { get; set; }

        public TabLayout? Tab { get; set; }
    }
}
