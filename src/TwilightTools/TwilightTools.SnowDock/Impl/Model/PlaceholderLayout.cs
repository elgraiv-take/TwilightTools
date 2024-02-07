using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.TwilightTools.SnowDock.Impl.Model;
internal class PlaceholderLayout : ILayout
{
    public static readonly PlaceholderLayout Instance = new();

    public IIntermediateLayout? Parent => null;

    public int ChildCount => 0;

    public void AddContent(LayoutPath path, LayoutContent content)
    {
        Debug.Assert(false);
    }
    public void OptimizeLayout()
    {
    }

#if DEBUG
    public void Debug_SerializeTo(TextWriter writer)
    {
        writer.WriteLine("_");
    }
#endif
}
