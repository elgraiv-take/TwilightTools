using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.TwilightTools.SnowDock
{
    public record LayoutPath
    {
        public ImmutableArray<uint> Path { get; }

        public uint FloatId { get; init; } = 0;

        public bool IsFloating => FloatId != 0;
        public int TabIndex { get; } = -1;
        public bool IsValid => !Path.IsEmpty;

        public static readonly LayoutPath Invalid = new LayoutPath(Enumerable.Empty<uint>());
        public LayoutPath()
        {
            Path = [0];
        }
        public LayoutPath(IEnumerable<uint> path)
        {
            Path = path.ToImmutableArray();
        }
        public LayoutPath(IEnumerable<uint> path, int tabIndex) : this(path)
        {
            TabIndex = tabIndex;
        }
    }
}
