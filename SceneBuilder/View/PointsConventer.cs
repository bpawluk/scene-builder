using SceneBuilder.Model;
using System;
using SceneBuilder.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using JeremyAnsel.DirectX.DXMath;

namespace SceneBuilder.View
{
    class PointsConventer : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            PointCollection result = new PointCollection();

            var vertex = values[0] as int[];
            var points = values[1] as XMVector[];

            if (vertex != null && points != null)
            {
                for (int i = 0; i < vertex.Length; i++)
                {
                    result.Add(new Point(points[vertex[i]].X, points[vertex[i]].Y));
                }
            }
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
