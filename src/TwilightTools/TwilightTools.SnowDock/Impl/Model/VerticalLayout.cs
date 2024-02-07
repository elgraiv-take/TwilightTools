using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.TwilightTools.SnowDock.Impl.Model
{
    internal class VerticalLayout : LayoutBase
    {
        public override LayoutOrientation Orientation => LayoutOrientation.Vertical;
        public VerticalLayout(HorizontalLayout parent, RootLayout root, int level) : base(parent, root, level)
        {
        }
        protected override LayoutBase CreateChild() => new HorizontalLayout(this,Root, Level + 1);

    }
}
