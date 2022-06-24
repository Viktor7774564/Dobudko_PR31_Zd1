using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BeautySalon.Converters
{
    internal class FinalPriceConverter : IMultiValueConverter
    {
     
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var Price = (decimal)values[0];
            var Discount = (double)values[1];

            var DFinalPrice = double.Parse(Price.ToString()) - double.Parse(Price.ToString()) * Discount;

            int FinalPrice = System.Convert.ToInt32(DFinalPrice);

            return FinalPrice.ToString();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
