using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Elgraiv.TwilightTools.SnowDock.Impl.Model
{
    internal class FloatingRoot
    {
        private RootLayout _root;
        public RootLayout Root => _root;
        public Rect WindowRect { get; set; }

        public FloatingRoot(uint id)
        {
            _root = new(id);
        }

        public void AddContent(LayoutPath path, LayoutContent content)
        {
            _root.AddContent(path, content);
        }
        public void OptimizeLayout()
        {
            _root.OptimizeLayout();
        }

        public void CloseAll()
        {

        }
    }
}
