using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

        public void InsertLeft(LayoutContent content)
        {
            var tab = new TabLayout(this, Root);
            Children.Insert(0,tab);
            tab.AddContent(tab.ComputeCurrentPath(), content);

            InvalidateLayout();
        }

        public void InsertRight(LayoutContent content)
        {
            var tab = new TabLayout(this, Root);
            Children.Add(tab);
            tab.AddContent(tab.ComputeCurrentPath(), content);

            InvalidateLayout();
        }

        public (VerticalLayout newVertical,HorizontalLayout newHorizontal) InsertLevel()
        {
            var parent = Parent as VerticalLayout;
            var horizontal = parent is null ? new HorizontalLayout(Root) : new HorizontalLayout(parent, Root, Level);
            var vertical = new VerticalLayout(horizontal, Root, Level + 1);
            horizontal.Children.Add(vertical);
            vertical.AddLayout(this);
            return (vertical, horizontal);
        }

        protected override LayoutBase CreateChild() => new VerticalLayout(this, Root, Level + 1);

    }
}
