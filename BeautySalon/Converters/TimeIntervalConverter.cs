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
    internal class TimeIntervalConverter : IValueConverter
    {
        public DateTime UpdatingDate { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var StartDate = (DateTime)value;

            if(StartDate >= DateTime.Now.Date)
            {
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(30);
                timer.Tick += Timer_Tick;
                timer.Start();

                UpdatingDate = DateTime.Now;

                var Interval = StartDate - UpdatingDate;

            

                if (Interval.Days > 0)
                {
                    var hours = Interval.Hours + Interval.Days * 24;
                    return $"{hours} часов {Interval.Minutes} минут";
                }
                else
                {
                    return $"{Interval.Hours} часов {Interval.Minutes} минут";
                }
            }
            return string.Empty;
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
