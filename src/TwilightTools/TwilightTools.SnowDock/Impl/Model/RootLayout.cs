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
        private HorizontalLayout _layout;
        public HorizontalLayout Layout => _layout;

        private HashSet<LayoutContent> _contents = new();
        public IReadOnlyCollection<LayoutContent> Contents => _contents;

        public event EventHandler? LayoutRequested;

        public uint FloatId { get; }

        public int ContentCount => _contents.Count;

        public RootLayout(uint floatingId)
        {
            _layout = new(this);
            FloatId = floatingId;
        }

        public void AddContent(LayoutPath path, LayoutContent content)
        {
            _contents.Add(content);
            _layout.AddContent(path, content);
        }
        public void OptimizeLayout()
        {
            _layout.OptimizeLayout();
            LayoutRequested?.Invoke(this, EventArgs.Empty);
        }

        public void RemoveContent(LayoutContent content)
        {
            _contents.Remove(content);
        }

        public void InsertLeft(LayoutContent content)
        {
            _contents.Add(content);
            Layout.InsertLeft(content);
        }

        public void InsertRight(LayoutContent content)
        {
            _contents.Add(content);
            Layout.InsertRight(content);
        }

        internal void InsertTop(LayoutContent content)
        {
            _contents.Add(content);
            var (vertical, horizontal) = _layout.InsertLevel();

            _layout = horizontal;
            vertical.InsertTop(content);
        }
        internal void InsertBottom(LayoutContent content)
        {

            _contents.Add(content);
            var (vertical, horizontal) = _layout.InsertLevel();

            _layout = horizontal;
            vertical.InsertBottom(content);
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
