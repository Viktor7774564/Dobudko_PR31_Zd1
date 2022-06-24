using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;

namespace BeautySalon.Converters
{
    public class ColIntervalConverter : IValueConverter
    {
        public DateTime UpdatingDate { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var StartDate = (DateTime)value;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(30);
            timer.Tick += Timer_Tick;
            timer.Start();

            UpdatingDate = DateTime.Now;

            var Interval = StartDate - UpdatingDate;

            if (Interval.Days == 0 && Interval.Hours == 0 && Interval.Minutes <= 59 && Interval.Minutes >= 0)
            {
                return Brushes.Red;
            }
            else
            {
                return Brushes.Black;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdatingDate = DateTime.Now;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
