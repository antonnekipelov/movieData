using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RecomendMovie.Converters
{
    public class CollectionToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null)
                    return string.Empty;
                if (value is IEnumerable collection)
                {
                    var nonNullItems = collection.Cast<object>().Where(item => item != null);
                    if (!nonNullItems.Any())
                        return string.Empty;
                    return string.Join(", ", nonNullItems);
                }
                throw new InvalidOperationException("Input value is not a valid IEnumerable.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in CollectionToStringConverter.Convert: {ex.Message}");
                return string.Empty;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
