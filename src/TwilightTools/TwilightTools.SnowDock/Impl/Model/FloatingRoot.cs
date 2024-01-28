using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.TwilightTools.SnowDock.Impl.Model
{
    internal class FloatingRoot
    {
        private RootLayout _root = new();
        public void AddContent(LayoutPath path, LayoutContent content)
        {
            _root.AddContent(path, content);
        }
        public void OptimizeLayout()
        {
            _root.OptimizeLayout();
        }
    }
}
