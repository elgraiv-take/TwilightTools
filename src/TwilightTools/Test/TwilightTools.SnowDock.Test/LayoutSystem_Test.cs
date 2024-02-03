using System.Diagnostics;
using System.IO;
using System.Text;
using Elgraiv.TwilightTools.SnowDock.Impl.Model;

namespace Elgraiv.TwilightTools.SnowDock.Test;
[TestClass]
public class LayoutSystem_Test
{
    private class PanelForTest(string id) : IDockPanelViewModel
    {
        public string Header => throw new NotImplementedException();

        public bool IsVisible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string ContentId =>id;

        public LayoutPath? PreferedPath => throw new NotImplementedException();
    }

    [TestMethod]
    public void Layout_Test()
    {
        var system = new LayoutSystem();
        var count = 0;
        var createId = () => new PanelForTest($"Test_{count++:d03}");

        system.AddContent(new LayoutPath([3, 1, 1, 0]), new LayoutContent(createId()));
        system.AddContent(new LayoutPath([3, 1, 1, 1]), new LayoutContent(createId()));
        system.AddContent(new LayoutPath([3, 1, 2, 2]), new LayoutContent(createId()));
        system.AddContent(new LayoutPath([0]), new LayoutContent(createId()));
        system.AddContent(new LayoutPath([0], 0), new LayoutContent(createId()));
        system.AddContent(new LayoutPath([1]), new LayoutContent(createId()));
        system.AddContent(new LayoutPath([1, 0]), new LayoutContent(createId()));
        system.AddContent(new LayoutPath([1, 1]), new LayoutContent(createId()));
        system.AddContent(new LayoutPath([1, 2, 0]), new LayoutContent(createId()));
        system.AddContent(new LayoutPath([1, 2]), new LayoutContent(createId()));
        system.AddContent(new LayoutPath([1, 2, 1]), new LayoutContent(createId()));
        system.AddContent(new LayoutPath([2]), new LayoutContent(createId()));

        system.OptimizeLayout();

        {
            using var stream = new MemoryStream();
            {
                using var writer = new StreamWriter(stream);
                system.Debug_SerializeTo(writer);
            }
            var str = Encoding.UTF8.GetString(stream.ToArray());

            Console.WriteLine(str);
        }


    }
}
