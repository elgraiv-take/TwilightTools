using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Elgraiv.TwilightTools.SnowDock.Impl;

namespace Elgraiv.TwilightTools.SnowDock.Controls;

public class DockingTargetIcon : Control
{
    static DockingTargetIcon()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DockingTargetIcon), new FrameworkPropertyMetadata(typeof(DockingTargetIcon)));
        EventManager.RegisterClassHandler(typeof(DockingTargetIcon), DockRootPanel.DockingTargetOverEvent, new DockingTargetEventHandler(OnDockingOverThunk), false);
    }

    //public DockingTargetPlace TargePlace { get; set; }


    public DockingTargetPlace TargetPlace
    {
        get { return (DockingTargetPlace)GetValue(TargetPlaceProperty); }
        set { SetValue(TargetPlaceProperty, value); }
    }

    public static readonly DependencyProperty TargetPlaceProperty =
        DependencyProperty.Register(nameof(DockingTargetPlace), typeof(DockingTargetPlace), typeof(DockingTargetIcon), new PropertyMetadata(DockingTargetPlace.Panel));




    public bool IsDockingTarget
    {
        get { return (bool)GetValue(IsDockingTargetProperty); }
        set { SetValue(IsDockingTargetProperty, value); }
    }

    public static readonly DependencyProperty IsDockingTargetProperty =
        DependencyProperty.Register(nameof(IsDockingTarget), typeof(bool), typeof(DockingTargetIcon), new PropertyMetadata(false));



    private static void OnDockingOverThunk(object? sender, DockingTargetEventArgs e)
    {
        if (e.Handled)
        {
            return;
        }
        if (sender is DockingTargetIcon self)
        {
            self.OnDockingOver(e);
        }
    }

    private void OnDockingOver(DockingTargetEventArgs e)
    {
        e.Handled = true;
        e.TargetIcon = this;
        IsDockingTarget = true;
    }

    internal void ClearOver()
    {
        IsDockingTarget = false;
    }
}
