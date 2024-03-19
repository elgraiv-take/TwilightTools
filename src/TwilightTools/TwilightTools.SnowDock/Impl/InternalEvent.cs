using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Elgraiv.TwilightTools.SnowDock.Controls;

namespace Elgraiv.TwilightTools.SnowDock.Impl;
internal class LayoutUpdatedEventArgs : EventArgs
{
}

public class DockingEventArgs : RoutedEventArgs
{
    public BranchPanel? TargetPanel { get; set; }
    public DockTabPanel? TargetTab { get; set; }
}
public delegate void DockingEventHandler(object? sender, DockingEventArgs e);

public class DockingTargetEventArgs : RoutedEventArgs
{
    public DockingTargetIcon? TargetIcon { get; set; }
}
public delegate void DockingTargetEventHandler(object? sender, DockingTargetEventArgs e);


public enum DockingTargetPlace
{
    Panel,
    PanelLeft,
    PanelTop,
    PanelRight,
    PanelBottom,
    RootLeft,
    RootTop,
    RootRight,
    RootBottom,
}
