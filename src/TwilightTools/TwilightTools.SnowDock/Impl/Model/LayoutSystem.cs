using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Elgraiv.TwilightTools.SnowDock.Impl.Model
{
    internal class FloatingWindowEventArgs(FloatingRoot target) : EventArgs
    {
        public FloatingRoot TargetModel { get; } = target;
    }

    internal delegate void FloatingWindowEventHandler(object o, FloatingWindowEventArgs e);

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
            FloatingRoot? newFloating = null;
            if (path.IsFloating)
            {
                if(!_floatings.TryGetValue(path.FloatId,out var floating))
                {
                    floating = new FloatingRoot();
                    newFloating = floating;
                    _floatings[path.FloatId] = floating;
                }
                floating.AddContent(path, content);
            }
            else
            {
                _root.AddContent(path, content);
            }

            if (newFloating is not null)
            {
                FloatingWindowAdded?.Invoke(this, new FloatingWindowEventArgs(newFloating));
            }
        }

        public void RequestFloating(LayoutContent content, Rect rect)
        {
            var newFloating = new FloatingRoot()
            {
                WindowRect = rect,
            };
            var floatId = 1u;
            while (_floatings.ContainsKey(floatId))
            {
                floatId++;
            }
            var path = new LayoutPath()
            {
                FloatId = floatId
            };
            _floatings.Add(floatId, newFloating);
            newFloating.AddContent(new LayoutPath(), content);
            newFloating.OptimizeLayout();
            FloatingWindowAdded?.Invoke(this, new FloatingWindowEventArgs(newFloating));
        }

        public void RequestDock(LayoutContent content)
        {

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
                FloatingWindowRemoved?.Invoke(this, new FloatingWindowEventArgs(default!));
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
