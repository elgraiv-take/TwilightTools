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
        private RootLayout _root = new(0);
        public RootLayout Root => _root;
        private Dictionary<uint, FloatingRoot> _floatings = new();

        private Dictionary<string, LayoutContent> _contents = new();

        public event FloatingWindowEventHandler? FloatingWindowAdded;
        public event FloatingWindowEventHandler? FloatingWindowRemoved;

        public void AddContent(LayoutPath path, LayoutContent content)
        {
            if (!_contents.TryAdd(content.Id, content))
            {
                return;
            }
            FloatingRoot? newFloating = null;
            if (path.IsFloating)
            {
                if (!_floatings.TryGetValue(path.FloatId, out var floating))
                {
                    floating = new FloatingRoot(path.FloatId);
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
            var floatId = 1u;
            while (_floatings.ContainsKey(floatId))
            {
                floatId++;
            }
            var newFloating = new FloatingRoot(floatId)
            {
                WindowRect = rect,
            };
            var path = new LayoutPath()
            {
                FloatId = floatId
            };
            _floatings.Add(floatId, newFloating);
            newFloating.AddContent(new LayoutPath(), content);
            newFloating.OptimizeLayout();
            FloatingWindowAdded?.Invoke(this, new FloatingWindowEventArgs(newFloating));
        }

        public void RequestDock(DockingTargetPlace targetPlace,TabLayout targetTab, LayoutContent content)
        {

            RootLayout root;
            
            if (targetTab.Root.FloatId != 0)
            {
                if (_floatings.TryGetValue(targetTab.Root.FloatId, out var floating))
                {
                    root = floating.Root;
                }
                else
                {
                    return;
                }
            }
            else
            {
                root = _root;
            }


            var oldId = content.Tab?.Root.FloatId ?? 0;
            switch (targetPlace)
            {
                case DockingTargetPlace.Panel:
                    {
                        root.AddContent(targetTab.ComputeCurrentPath(), content);
                    }
                    break;
                case DockingTargetPlace.PanelLeft:
                    {

                    }
                    break;
                case DockingTargetPlace.PanelTop:
                    {

                    }
                    break;
                case DockingTargetPlace.PanelRight:
                    {

                    }
                    break;
                case DockingTargetPlace.PanelBottom:
                    {

                    }
                    break;
                case DockingTargetPlace.RootLeft:
                    {
                        root.InsertLeft(content);
                    }
                    break;
                case DockingTargetPlace.RootTop:
                    {
                        root.InsertTop(content);
                    }
                    break;
                case DockingTargetPlace.RootRight:
                    {
                        root.InsertRight(content);
                    }
                    break;
                case DockingTargetPlace.RootBottom:
                    {
                        root.InsertBottom(content);
                    }
                    break;
                default:
                    return;
            }

            root.OptimizeLayout();

            if (oldId != 0)
            {
                if (_floatings.TryGetValue(oldId, out var removing) && removing.Root.ContentCount <= 0)
                {
                    _floatings.Remove(oldId);
                    FloatingWindowRemoved?.Invoke(this, new FloatingWindowEventArgs(removing));
                }
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
                FloatingWindowRemoved?.Invoke(this, new FloatingWindowEventArgs(default!));
            }
        }


        public void OptimizeLayout()
        {
            _root.OptimizeLayout();
            foreach (var (_, floating) in _floatings)
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
