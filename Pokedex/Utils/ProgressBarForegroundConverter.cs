using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Pokedex.Converters
{
    [ValueConversion(typeof(int), typeof(SolidColorBrush))]
    public class ProgressBarForegroundConverter : IValueConverter
    {
        //Changing the color of the progress bar according to the numbers.
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            double progress = (double)value;
            SolidColorBrush foreground = new SolidColorBrush(Colors.Green);

            if (progress <= 85)
            {
                foreground = new SolidColorBrush(Colors.OrangeRed);
            }
            else if (progress <= 170)
            {
                foreground = new SolidColorBrush(Colors.Yellow);
            }
            else
            {
                foreground = new SolidColorBrush(Colors.LightGreen);
            }

            return foreground;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
