using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace GitHubReposExplorer.Views.Converters
{
    public class ItemTappedArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var itemTappedEventArgs = value as ItemTappedEventArgs;

            if (itemTappedEventArgs == null)

                return null;

            return itemTappedEventArgs.Item;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
