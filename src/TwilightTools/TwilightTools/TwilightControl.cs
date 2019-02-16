using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Elgraiv.TwilightTools
{
    public static class TwilightControl
    {
        public static readonly DependencyProperty CornerRadiusProperty =
           DependencyProperty.RegisterAttached(
               "CornerRadius",
               typeof(CornerRadius),
               typeof(TwilightControl),
               new PropertyMetadata(new CornerRadius(0.0)));

        public static CornerRadius GetCornerRadius(DependencyObject obj)
        {
            return (CornerRadius)obj.GetValue(CornerRadiusProperty);
        }

        public static void SetCornerRadius(DependencyObject obj, CornerRadius value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }
    }
}
