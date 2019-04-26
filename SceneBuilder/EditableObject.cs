using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneBuilder
{
    class EditableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(params string[] nazwyWłasnosci)
        {
            foreach (string wlasnosc in nazwyWłasnosci)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(wlasnosc));
            }
        }
    }
}
