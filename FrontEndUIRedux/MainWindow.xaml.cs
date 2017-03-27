using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;
using Microsoft.Kinect;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.Diagnostics;
using System.IO;
using System.Timers;

namespace FrontEndUIRedux
{
    public partial class MainWindow : Window
    {

        static Pipe guiClient = new Pipe("interface", true);
        static Pipe guiCommands = new Pipe("frominterface", true);
        Pipe[] pipes = { guiClient, guiCommands };

        Timer infoUpdateTimer = new Timer() { Interval = 1, Enabled = false };
        Timer infoResetTimer = new Timer() { Interval = 500, Enabled = false };
        Timer initalLoadTimer = new Timer() { Interval = 1500, Enabled = true };
        string message = "";

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Execute start up tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            infoUpdateTimer.Elapsed += new ElapsedEventHandler(infoUpdateTimer_Elapsed);
            infoResetTimer.Elapsed += new ElapsedEventHandler(infoResetTimer_Elapsed);
            initalLoadTimer.Elapsed += new ElapsedEventHandler(initalLoadTimer_Elapsed);
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            stopPrograms();
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
        public void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (infoUpdateTimer.Enabled)
            {
                infoUpdateTimer.Enabled = false;
                infoResetTimer.Enabled = true;
                foreach (Pipe pipe in pipes) { pipe.stop(); }
                this.Dispatcher.Invoke(() =>
                {
                    StartButton.IsEnabled = false;
                    ExportButton.IsEnabled = false;
                    StartButton.Content = "Start";
                });
            }
            else
            {
                foreach (Pipe pipe in pipes) { pipe.start(); }
                guiClient.write.WriteLine("Start");
                infoUpdateTimer.Enabled = true;
                this.Dispatcher.Invoke(() =>
                {
                    StartButton.Content = "Stop";
                    ExportButton.IsEnabled = true;
                });
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string,bool> stances = new Dictionary<string, bool>(){
                { "SingleLegStanceRadio", SingleLegStanceRadio.IsChecked == true },
                { "DoubleLegStanceRadio", DoubleLegStanceRadio.IsChecked == true },
                { "TandemLegStanceRadio", TandemLegStanceRadio.IsChecked == true }};

            ExportButton.IsEnabled = false;
            ExportButton.Content = "Collecting Data...";
            foreach(KeyValuePair<string,bool> stance in stances) { if (stance.Value) { guiCommands.write.WriteLine(stance.Key); } }
            
        }
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void MenuItem_board_Click_1(object sender, RoutedEventArgs e)
        {
            BoardWindow popup = new BoardWindow();
            popup.ShowDialog();
        }

        void infoUpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                parse(guiClient.read.ReadLine());
            });
        }

        private void parse(string line)
        {
            char[] delim = { ',', ';' };
            string[] words = line.Split(delim);
            if (words.Length > 2)
            {
                switch (words[1])
                {
                    case "RWeight":
                        RWeight.Content = words[2];
                        break;
                    case "TopLeft":
                        TLeft.Content = words[2];
                        break;
                    case "TopRight":
                        TRight.Content = words[2];
                        break;
                    case "BottomRight":
                        BRight.Content = words[2];
                        break;
                    case "BottomLeft":
                        BLeft.Content = words[2];
                        break;
                    case "spinebase":
                        SpineBaseX.Text = words[2];
                        SpineBaseY.Text = words[3];
                        SpineBaseZ.Text = words[4];
                        break;
                    case "midspine":
                        MidSpineX.Text = words[2];
                        MidSpineY.Text = words[3];
                        MidSpineZ.Text = words[4];
                        break;
                    case "neck":
                        NeckX.Text = words[2];
                        NeckY.Text = words[3];
                        NeckZ.Text = words[4];
                        break;
                    case "shoulderleft":
                        ShoulderLeftX.Text = words[2];
                        ShoulderLeftY.Text = words[3];
                        ShoulderLeftZ.Text = words[4];
                        break;
                    case "elbowleft":
                        ElbowLeftX.Text = words[2];
                        ElbowLeftY.Text = words[3];
                        ElbowLeftZ.Text = words[4];
                        break;
                    case "wristleft":
                        WristLeftX.Text = words[2];
                        WristLeftY.Text = words[3];
                        WristLeftZ.Text = words[4];
                        break;
                    case "handleft":
                        HandLeftX.Text = words[2];
                        HandLeftY.Text = words[3];
                        HandLeftZ.Text = words[4];
                        break;
                    case "head":
                        HeadX.Text = words[2];
                        HeadY.Text = words[3];
                        HeadZ.Text = words[4];
                        break;
                    case "shoulderright":
                        ShoulderRightX.Text = words[2];
                        ShoulderRightY.Text = words[3];
                        ShoulderRightZ.Text = words[4];
                        break;
                    case "elbowright":
                        ElbowRightX.Text = words[2];
                        ElbowRightY.Text = words[3];
                        ElbowRightZ.Text = words[4];
                        break;
                    case "handright":
                        HandRightX.Text = words[2];
                        HandRightY.Text = words[3];
                        HandRightZ.Text = words[4];
                        break;
                    case "hipleft":
                        HipLeftX.Text = words[2];
                        HipLeftY.Text = words[3];
                        HipLeftZ.Text = words[4];
                        break;
                    case "kneeleft":
                        KneeLeftX.Text = words[2];
                        KneeLeftY.Text = words[3];
                        KneeLeftZ.Text = words[4];
                        break;
                    case "footleft":
                        FootLeftX.Text = words[2];
                        FootLeftY.Text = words[3];
                        FootLeftZ.Text = words[4];
                        break;
                    case "hipright":
                        HipRightX.Text = words[2];
                        HipRightY.Text = words[3];
                        HipRightZ.Text = words[4];
                        break;
                    case "kneeright":
                        KneeRightX.Text = words[2];
                        KneeRightY.Text = words[3];
                        KneeRightZ.Text = words[4];
                        break;
                    case "footright":
                        FootRightX.Text = words[2];
                        FootRightY.Text = words[3];
                        FootRightZ.Text = words[4];
                        break;
                    case "wristright":
                        WristRightX.Text = words[2];
                        WristRightY.Text = words[3];
                        WristRightZ.Text = words[4];
                        break;
                    default:
                        break;
                }
            }
        }
        private void infoResetTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            infoResetTimer.Enabled = false;
            this.Dispatcher.Invoke(() =>
            {
                reset();
                StartButton.IsEnabled = true;
            });

        }
        private void reset()
        {
            string blank = "";
            RWeight.Content = blank;
            TLeft.Content = blank;
            TRight.Content = blank;
            BRight.Content = blank;
            BLeft.Content = blank;
            SpineBaseX.Text = blank;
            SpineBaseY.Text = blank;
            SpineBaseZ.Text = blank;
            MidSpineX.Text = blank;
            MidSpineY.Text = blank;
            MidSpineZ.Text = blank;
            NeckX.Text = blank;
            NeckY.Text = blank;
            NeckZ.Text = blank;
            ShoulderLeftX.Text = blank;
            ShoulderLeftY.Text = blank;
            ShoulderLeftZ.Text = blank;
            ElbowLeftX.Text = blank;
            ElbowLeftY.Text = blank;
            ElbowLeftZ.Text = blank;
            WristLeftX.Text = blank;
            WristLeftY.Text = blank;
            WristLeftZ.Text = blank;
            HandLeftX.Text = blank;
            HandLeftY.Text = blank;
            HandLeftZ.Text = blank;
            HeadX.Text = blank;
            HeadY.Text = blank;
            HeadZ.Text = blank;
            ShoulderRightX.Text = blank;
            ShoulderRightY.Text = blank;
            ShoulderRightZ.Text = blank;
            ElbowRightX.Text = blank;
            ElbowRightY.Text = blank;
            ElbowRightZ.Text = blank;
            HandRightX.Text = blank;
            HandRightY.Text = blank;
            HandRightZ.Text = blank;
            HipLeftX.Text = blank;
            HipLeftY.Text = blank;
            HipLeftZ.Text = blank;
            KneeLeftX.Text = blank;
            KneeLeftY.Text = blank;
            KneeLeftZ.Text = blank;
            FootLeftX.Text = blank;
            FootLeftY.Text = blank;
            FootLeftZ.Text = blank;
            HipRightX.Text = blank;
            HipRightY.Text = blank;
            HipRightZ.Text = blank;
            KneeRightX.Text = blank;
            KneeRightY.Text = blank;
            KneeRightZ.Text = blank;
            FootRightX.Text = blank;
            FootRightY.Text = blank;
            FootRightZ.Text = blank;
            WristRightX.Text = blank;
            WristRightY.Text = blank;
            WristRightZ.Text = blank;
        }

        private void initalLoadTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                StartButton.IsEnabled = true;
            });
            
        }

        static void stopPrograms()
        {
        List<string> programList = new List<string>()
            {"KinectEnvironment","BalanceBoard","Backend","Tunnel",
             //"testapp","testapp2"
            };
            char[] del = { '\\', '.' };
            Parallel.ForEach(programList, program => {
                foreach (var process in Process.GetProcessesByName(program))
                {
                    process.Kill();
                }
            });
        }
    }



    }

