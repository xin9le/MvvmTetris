using System;
using System.Globalization;
using System.Windows.Data;



namespace MvvmTetris.Wpf.Converters
{
    /// <summary>
    /// <see cref="System.Drawing.Color"/> から <seealso cref="System.Windows.Media.Color"/> への変換機能を提供します。
    /// </summary>
    internal class DrawingToMediaColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var x = (System.Drawing.Color)value;
            return System.Windows.Media.Color.FromArgb(x.A, x.R, x.G, x.B);
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
