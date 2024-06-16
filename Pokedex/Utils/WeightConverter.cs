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
    class WeightConverter : IValueConverter
    {
        //Convert the pokemon weight to kgs and lbs.
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            double kg = (double)value;
            double lbs = kg * 2.2;
            return string.Format("{0}kg / {1}lbs",kg,Math.Round(lbs,1));
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
