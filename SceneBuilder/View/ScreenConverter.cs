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
    class ScreenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PointCollection result = new PointCollection();
            var points = value as XMVector[];

            if (points != null)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    result.Add(new Point(points[i].X, points[i].Y));
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
