using System;
using Windows.UI.Xaml.Data;

namespace Covid19Analysis.Converter
{
    internal class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var isNumeric = int.TryParse(value.ToString(), out var number);
            return isNumeric ? number : -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value.ToString();
        }
    }
}
