using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.TwilightTools.SnowDock.Impl.Model;

internal abstract class LayoutBase : IIntermediateLayout
{
    protected List<ILayout> Children { get; } = new();
    protected LayoutBase? Parent { get; }
    protected int Level { get; private set; }
    public abstract LayoutOrientation Orientation { get; }
    public int ChildCount => Children.Count;

    ILayout? ILayout.Parent => Parent;


    public LayoutBase(LayoutBase? parent, int level)
    {
        Parent = parent;
        Level = level;
    }

    public void AddContent(LayoutPath path, LayoutContent content)
    {
        var index = (int)path.Path.ElementAtOrDefault(Level);

        bool endpoint = Level >= path.Path.Length - 1;
        var insufficient = (index + 1) - Children.Count;
        for (var i = 0; i < insufficient; i++)
        {
            Children.Add(PlaceholderLayout.Instance);
        }
        var target = Children[index];

        if (endpoint)
        {
            switch (target)
            {
                case TabLayout:
                case IIntermediateLayout:
                    target.AddContent(path, content);
                    break;
                default:
                    {
                        var tab = new TabLayout(this);
                        Children[index] = tab;
                        tab.AddContent(path, content);
                    }
                    break;
            }
        }
        else
        {
            switch (target)
            {
                case TabLayout tab:
                    {
                        var child = CreateChild();
                        Children[index] = child;
                        child.MoveTab(tab);
                        child.AddContent(path, content);
                    }
                    break;
                case IIntermediateLayout intermediate:
                    intermediate.AddContent(path, content);
                    break;
                default:
                    {
                        var child = CreateChild();
                        Children[index] = child;
                        child.AddContent(path, content);
                    }
                    break;
            }
        }
    }

    protected abstract LayoutBase CreateChild();

    public void OptimizeLayout()
    {
        foreach (var child in Children)
        {
            child.OptimizeLayout();
        }
        while (Children.Remove(PlaceholderLayout.Instance))
        {

        }

        bool needSquash = Children.Any((child) => child is IIntermediateLayout intermediate && intermediate.ChildCount <= 1);
        if (needSquash)
        {
            var newChildren = new List<ILayout>();
            foreach(var child in Children)
            {
                if (child is LayoutBase intermediate && intermediate.ChildCount <= 1)
                {
                    intermediate.SquashChildren(newChildren);
                }
                else
                {
                    newChildren.Add(child);
                }
            }
            foreach(var child in newChildren)
            {
                if(child is TabLayout tab)
                {
                    tab.Parent = this;
                }
            }

            Children.Clear();
            Children.AddRange(newChildren);
        }
    }

    private void DeindentLevel(int level)
    {
        Level -= level;
        foreach (var child in Children)
        {
            if (child is LayoutBase imd)
            {
                imd.DeindentLevel(level);
            }
        }
    }

    public void SquashChildren(ICollection<ILayout> target)
    {
        Debug.Assert(Children.Count == 1, $"{Children.Count}");

        var child = Children.First();
        if( child is LayoutBase imd)
        {
            foreach (var gc in imd.Children)
            {
                if (gc is LayoutBase imd2)
                {
                    imd2.DeindentLevel(2);
                    target.Add(imd2);
                }
                else
                {
                    target.Add(gc);
                }
            }
        }
        else
        {
            target.Add(child);
        }

    }

    public void MoveTab(TabLayout tab)
    {
        tab.Parent = this;
        Children.Add(tab);
    }
#if DEBUG
    int IIntermediateLayout.Level => Level;

    public void Debug_SerializeTo(TextWriter writer)
    {
        writer.WriteLine($"{new string(' ', Level * 2)}- {GetType().Name}");
        foreach(var child in Children)
        {
            child.Debug_SerializeTo(writer);
        }
    }
#endif
}
