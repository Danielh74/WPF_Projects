using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Pokedex.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    class HeightConverter : IValueConverter
    {
        //Convert the pokemon height data to meters and feet and inches
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            double meters = (double)value;
            double feet = meters / 0.3048;
            double inches = (feet - Math.Truncate(feet)) * 12;
            return string.Format("{0}m / {1}'{2}\"",meters, Math.Truncate(feet), Math.Round(inches));
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
