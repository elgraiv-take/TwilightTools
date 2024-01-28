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
        internal HorizontalLayout(VerticalLayout parent, int level) : base(parent, level)
        {
        }
        internal HorizontalLayout() : base(null, 0)
        {
        }

        protected override LayoutBase CreateChild() => new VerticalLayout(this, Level + 1);
        
    }
}
