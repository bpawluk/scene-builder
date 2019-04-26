using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SceneBuilder.Model
{
    struct Triangle3D
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int[] Points { get; set; }

        public void Refresh()
        {
            OnPropertyChanged("Points");
        }

        public bool IsValid()
        {
            return Points.Length == 3 && Points[0] >= 0 && Points[1] >= 0 && Points[2] >= 0;
        }

        private void OnPropertyChanged(params string[] propertiesNames)
        {
            foreach (string wlasnosc in propertiesNames)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(wlasnosc));
            }
        }
    }
}