using System;
using System.Collections.Generic;
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
//using System;
//using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
//using System.Windows;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
using Microsoft.Kinect;

namespace FrontEndUIRedux
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            polyline.Points.Add(e.GetPosition(canvas));
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SingleLegRadio_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void DoubleLegRadio_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void TandemLegRadio_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

    }
}

