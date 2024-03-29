﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.TwilightTools.SnowDock.Impl.Model
{
    internal interface ILayout
    {
        public IIntermediateLayout? Parent { get; }

        public int ChildCount { get; }
        public void AddContent(LayoutPath path, LayoutContent content);
        public void OptimizeLayout();

#if DEBUG
        public void Debug_SerializeTo(TextWriter writer);
#endif
    }

    internal enum LayoutOrientation
    {
        Horisontal,
        Vertical,
    }
    internal interface IIntermediateLayout : ILayout
    {
        public event EventHandler? ReconstructRequested;
        public LayoutOrientation Orientation { get; }

        public IReadOnlyCollection<ILayout> Children { get; }

        public bool IsValidLayout { get; }

        public void InvalidateLayout();
        public void RemoveTab(TabLayout tab);

        public int GetChildIndex(ILayout child);

#if DEBUG
        public int Level { get; }
#endif
    }
}
