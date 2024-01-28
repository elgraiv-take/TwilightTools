using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.TwilightTools.SnowDock.Impl.Model
{
    public class FloatingWindowEventArgs : EventArgs
    {

    }

    public delegate void FloatingWindowEventHandler(object o, FloatingWindowEventArgs e);

    internal class LayoutSystem
    {
        private RootLayout _root = new();
        public RootLayout Root => _root;
        private Dictionary<uint, FloatingRoot> _floatings = new();

        private Dictionary<string, LayoutContent> _contents = new();

        public event FloatingWindowEventHandler? FloatingWindowAdded;
        public event FloatingWindowEventHandler? FloatingWindowRemoved;

        public void AddContent(LayoutPath path,LayoutContent content)
        {
            if (!_contents.TryAdd(content.Id, content))
            {
                return;
            }
            var newFloating = false;
            if (path.IsFloating)
            {
                if(!_floatings.TryGetValue(path.FloatId,out var floating))
                {
                    floating = new FloatingRoot();
                    newFloating = true;
                    _floatings[path.FloatId] = floating;
                }
                floating.AddContent(path, content);
            }
            else
            {
                _root.AddContent(path, content);
            }

            if (newFloating)
            {
                FloatingWindowAdded?.Invoke(this, new FloatingWindowEventArgs());
            }
        }

        public void RemoveContent(LayoutContent content)
        {
            if (!_contents.Remove(content.Id))
            {
                return;
            }
            var removeFloating = false;
            if (removeFloating)
            {
                FloatingWindowRemoved?.Invoke(this, new FloatingWindowEventArgs());
            }
        }


        public void OptimizeLayout()
        {
            _root.OptimizeLayout();
            foreach(var (_,floating) in _floatings)
            {
                floating.OptimizeLayout();
            }
        }

        public void Reset()
        {

        }

#if DEBUG
        public void Debug_SerializeTo(TextWriter writer)
        {
            _root.Debug_SerializeTo(writer);
        }
#endif
    }
}
