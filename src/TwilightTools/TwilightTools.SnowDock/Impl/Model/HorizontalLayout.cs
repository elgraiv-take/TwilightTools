using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.TwilightTools.SnowDock.Impl.Model
{
    internal class HorizontalLayout : LayoutBase
    {
        public override LayoutOrientation Orientation => LayoutOrientation.Horisontal;

        internal HorizontalLayout(VerticalLayout parent, RootLayout root, int level) : base(parent, root, level)
        {
        }
        internal HorizontalLayout(RootLayout root) : base(null, root, 0)
        {
        }

        protected override LayoutBase CreateChild() => new VerticalLayout(this, Root, Level + 1);

    }
}
