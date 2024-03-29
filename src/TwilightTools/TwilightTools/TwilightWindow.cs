﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Elgraiv.TwilightTools;

public static class TwilightWindow
{
    public static readonly DependencyProperty WindowIconProperty =
    DependencyProperty.RegisterAttached(
        "WindowIcon",
        typeof(object),
        typeof(TwilightWindow),
        new PropertyMetadata(null));

    public static object GetWindowIcon(DependencyObject obj)
    {
        return obj.GetValue(WindowIconProperty);
    }

    public static void SetWindowIcon(DependencyObject obj, object value)
    {
        obj.SetValue(WindowIconProperty, value);
    }

    public static readonly DependencyProperty HeaderBackgroundProperty =
    DependencyProperty.RegisterAttached(
        "HeaderBackground",
        typeof(Brush),
        typeof(TwilightWindow),
        new PropertyMetadata(null));

    public static Brush GetHeaderBackground(DependencyObject obj)
    {
        return (Brush)obj.GetValue(HeaderBackgroundProperty);
    }

    public static void SetHeaderBackground(DependencyObject obj, Brush value)
    {
        obj.SetValue(HeaderBackgroundProperty, value);
    }

    public static readonly DependencyProperty HeaderForegroundProperty =
    DependencyProperty.RegisterAttached(
        "HeaderForeground",
        typeof(Brush),
        typeof(TwilightWindow),
        new PropertyMetadata(null));

    public static Brush GetHeaderForeground(DependencyObject obj)
    {
        return (Brush)obj.GetValue(HeaderBackgroundProperty);
    }

    public static void SetHeaderForeground(DependencyObject obj, Brush value)
    {
        obj.SetValue(HeaderBackgroundProperty, value);
    }

    public static readonly DependencyProperty DialogResultProperty = DependencyProperty.RegisterAttached(
        "DialogResult",
        typeof(bool?),
        typeof(TwilightWindow),
        new PropertyMetadata(null, OnDialogResultChanged));

    public static bool GetDialogResult(DependencyObject obj)
    {
        return (bool)obj.GetValue(DialogResultProperty);
    }
    public static void SetDialogResult(DependencyObject obj, bool value)
    {
        obj.SetValue(DialogResultProperty, value);
    }

    private static void OnDialogResultChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is Window window)
        {
            window.DialogResult = GetDialogResult(window);
        }
    }
}
