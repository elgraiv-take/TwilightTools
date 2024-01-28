using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.TwilightTools.SnowDock.Impl.Model
{
    internal class RootLayout
    {
        private HorizontalLayout _layout = new();
        public HorizontalLayout Layout => _layout;

        public void AddContent(LayoutPath path, LayoutContent content)
        {
            _layout.AddContent(path, content);
        }
        public void OptimizeLayout()
        {
            _layout.OptimizeLayout();
        }


#if DEBUG
        public void Debug_SerializeTo(TextWriter writer)
        {
            writer.WriteLine($"* RootLayout");
            _layout.Debug_SerializeTo(writer);
        }
#endif
    }
}
