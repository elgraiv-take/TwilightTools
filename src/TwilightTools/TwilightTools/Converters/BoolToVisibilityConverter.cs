using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Elgraiv.TwilightTools.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public Visibility VisivilityTrue { get; set; } = Visibility.Visible;
        public Visibility VisivilityFalse { get; set; } = Visibility.Collapsed;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool flag)
            {
                if (flag)
                {
                    return VisivilityTrue;
                }
            }
            return VisivilityFalse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
