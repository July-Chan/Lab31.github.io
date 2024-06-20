using System;
using System.Globalization;
using System.Windows.Data;

namespace Lab31
{ // Клас для перетворення значень типу long у мегабайти
    public class ByteToMBConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long bytes)
            {
                return (bytes / 1024f / 1024f).ToString("F2");
            }
            return "0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
