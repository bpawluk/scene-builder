using JeremyAnsel.DirectX.DXMath;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace SceneBuilder.View
{
    class PointToSquareConventer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PointCollection result = new PointCollection();
            var point = (XMVector)value;

            if (point != null)
            {
                result.Add(new Point(point.X - 6, point.Y - 6));
                result.Add(new Point(point.X - 6, point.Y + 6));
                result.Add(new Point(point.X + 6, point.Y + 6));
                result.Add(new Point(point.X + 6, point.Y - 6));
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
