using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SceneBuilder.Model
{
    class Point3D : EditableObject
    {
        private double _x;
        private double _y;
        private double _z;

        public double X
        {
            get => _x;
            set
            {
                _x = value;
                OnPropertyChanged("X");
            }
        }
        public double Y
        {
            get => _y;
            set
            {
                _y = value;
                OnPropertyChanged("Y");
            }
        }
        public double Z
        {
            get => _z;
            set
            {
                _z = value;
                OnPropertyChanged("Z");
            }
        }
        public Point3D(double x, double y, double z)
        {
            _x = x;
            _y = y;
            _z = z;
        }
    }
}
