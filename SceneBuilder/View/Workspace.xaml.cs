using JeremyAnsel.DirectX.DXMath;
using Microsoft.Win32;
using SceneBuilder.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SceneBuilder.View
{
    /// <summary>
    /// Logika interakcji dla klasy Workspace.xaml
    /// </summary>
    public partial class Workspace : Page
    {
        public Workspace()
        {
            InitializeComponent();
        }

        private void LoadFile(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this) as MainWindow;
            var app = Application.Current as App;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true) app.FilePath = openFileDialog.FileName;
            (this.DataContext as WorkspaceVM).Initialize();
        }

        private void LoadCam(object sender, RoutedEventArgs e)
        {
            var context = this.DataContext as WorkspaceVM;

            if (context != null)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    var lines = File.ReadLines(openFileDialog.FileName);
                    var camPos = lines.ElementAt(0).Split(' ');
                    var scrCen = lines.ElementAt(1).Split(' ');
                    var angle = lines.ElementAt(2);
                    XMVector cameraPosition = new XMVector();
                    XMVector screenCenter = new XMVector();
                    int viewAngle = 90;
                    if (camPos.Length >= 3) cameraPosition = new XMVector(float.Parse(camPos[0], CultureInfo.InvariantCulture), float.Parse(camPos[1], CultureInfo.InvariantCulture), float.Parse(camPos[2], CultureInfo.InvariantCulture), 1);
                    if (scrCen.Length >= 3) screenCenter = new XMVector(float.Parse(scrCen[0], CultureInfo.InvariantCulture), float.Parse(scrCen[1], CultureInfo.InvariantCulture), float.Parse(scrCen[2], CultureInfo.InvariantCulture), 1);
                    int.TryParse(angle, out viewAngle);
                    context.LoadCamera(cameraPosition, screenCenter, viewAngle);
                }
            }
        }

        private void SaveCam(object sender, RoutedEventArgs e)
        {
            var context = this.DataContext as WorkspaceVM;

            if (context != null)
            {
                string fileText = $"{context.CameraOrigin.X.ToString(CultureInfo.InvariantCulture)} {context.CameraOrigin.Y.ToString(CultureInfo.InvariantCulture)} {context.CameraOrigin.Z.ToString(CultureInfo.InvariantCulture)}" +
                                  $"\n{context.ScreenCenter.X.ToString(CultureInfo.InvariantCulture)} {context.ScreenCenter.Y.ToString(CultureInfo.InvariantCulture)} {context.ScreenCenter.Z.ToString(CultureInfo.InvariantCulture)}" +
                                  $"\n{context.ViewAngle}";

                SaveFileDialog dialog = new SaveFileDialog();

                if (dialog.ShowDialog() == true)
                {
                    File.WriteAllLines(dialog.FileName, fileText.Split('\n'));
                }
            }
        }
    }
}
