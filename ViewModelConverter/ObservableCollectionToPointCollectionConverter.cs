using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CustomSpectrumAnalyzer
{

    // Converter
    public class ObservableCollectionToPointCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var points = value as ObservableCollection<Point>;
            if (points == null)
            {
                return null;
            }

            var collection = new PointCollection();
            foreach (Point point in points)
            {
                collection.Add(point);
            }

            return collection;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Not needed for one-way bindings.
            throw new NotImplementedException();
        }
    }
}
