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

        public IIntermediateLayout Parent { get; set; }

        ILayout? ILayout.Parent => Parent;

        public int ChildCount => _contents.Count;

        public TabLayout(IIntermediateLayout layout)
        {
            Parent = layout;
        }

        public void AddContent(LayoutPath path, LayoutContent content)
        {
            var index = path.TabIndex;

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
                writer.WriteLine($"{new string(' ', (level+1) * 2)}+ {content.Id}");
            }
        }
#endif
    }
}
