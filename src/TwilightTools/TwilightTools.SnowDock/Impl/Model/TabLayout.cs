using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.TwilightTools.SnowDock.Impl.Model
{
    internal class TabLayout : ILayout
    {
        private ObservableCollection<LayoutContent> _contents = new();
        public ReadOnlyObservableCollection<LayoutContent> Contents { get; }

        internal Guid InternalId { get; } = Guid.NewGuid();

        public IIntermediateLayout Parent { get; set; }

        public int ChildCount => _contents.Count;

        public RootLayout Root { get; }

        public event EventHandler? TabDeleting;


        public TabLayout(IIntermediateLayout layout, RootLayout root)
        {
            Parent = layout;
            Contents = new(_contents);
            Root = root;
        }

        public void RemoveContent(LayoutContent content)
        {
            if (_contents.Contains(content))
            {
                _contents.Remove(content);
                content.Tab = null;
                Root.RemoveContent(content);
            }

            if (_contents.Count < 1)
            {
                TabDeleting?.Invoke(this, EventArgs.Empty);
                //タブの削除
                Parent.RemoveTab(this);


                Root.OptimizeLayout();
            }
        }

        public void AddContent(LayoutPath path, LayoutContent content)
        {
            var index = path.TabIndex;

            if (content.Tab == this)
            {
                return;
            }

            if (content.Tab is not null)
            {
                content.Tab.RemoveContent(content);
            }


            if (index < 0)
            {
                _contents.Add(content);
            }
            else
            {
                var insertIndex = 0;
                for (; insertIndex < _contents.Count; insertIndex++)
                {
                    var expectIndex = _contents[insertIndex].ExpectedPath?.TabIndex ?? -1;
                    if (expectIndex < 0)
                    {
                        break;
                    }
                    if (index > expectIndex)
                    {
                        break;
                    }
                }
                _contents.Insert(insertIndex, content);
            }
            content.Tab = this;
        }

        public LayoutPath ComputeCurrentPath()
        {
            ILayout current = this;
            IIntermediateLayout? parent = Parent;

            var path = new List<uint>();

            while (parent is not null)
            {
                var index = parent.GetChildIndex(current);
                if (index < 0)
                {
                    return new LayoutPath();
                }
                path.Add((uint)index);
                current = parent;
                parent = parent.Parent;
            }
            path.Reverse();

            return new LayoutPath(path) { FloatId = Root.FloatId };
        }
        public void OptimizeLayout()
        {
        }


#if DEBUG
        public void Debug_SerializeTo(TextWriter writer)
        {
            var level = Parent.Level + 1;
            writer.WriteLine($"{new string(' ', level * 2)}- Tab");
            foreach (var content in _contents)
            {
                writer.WriteLine($"{new string(' ', (level + 1) * 2)}+ {content.Id}");
            }
        }
#endif
    }
}
