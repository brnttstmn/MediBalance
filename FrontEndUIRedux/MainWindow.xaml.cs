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
using System.Windows.Shapes;

namespace FrontEndUIRedux
{
    public partial class MainWindow : Window
    {
        // Create Pipe Objects
        static Pipe guiClient = new Pipe("interface", true);
        static Pipe guiCommands = new Pipe("frominterface", true);
        Pipe[] pipes = { guiClient, guiCommands };

        // Set Timers
        Timer infoUpdateTimer = new Timer() { Interval = .1, Enabled = false };
        Timer infoResetTimer = new Timer() { Interval = 500, Enabled = false };
        Timer initalLoadTimer = new Timer() { Interval = 1500, Enabled = true };
        //Timer graphTimer = new Timer() { Interval = 100, Enabled = false };
        private System.Windows.Threading.DispatcherTimer timer;

        // Button Presses
        public void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (infoUpdateTimer.Enabled)
            {
                stop();
            }
            else
            {
                start();
            }
        }
        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            guiCommands.start();
            Dictionary<string,bool> stances = new Dictionary<string, bool>(){
                { "SingleLegStanceRadio", SingleLegStanceRadio.IsChecked == true },
                { "DoubleLegStanceRadio", DoubleLegStanceRadio.IsChecked == true },
                { "TandemLegStanceRadio", TandemLegStanceRadio.IsChecked == true }};

            ExportButton.IsEnabled = false;
            ExportButton.Content = "Collecting Data...";
            foreach(KeyValuePair<string,bool> stance in stances) { if (stance.Value) { guiCommands.write.WriteLine(stance.Key); } }
            
        }

        //Ellipse initialization
        Ellipse[] bodypoints = new Ellipse[18];

        

        // Array of Joint locations --------------------------------------
        double[,] joints = new double[18,2];
        //int[,] joints = new int[,] { { 0, 0 },
        //                             { 0, 5 },
        //                             { 0, 10 },
        //                             { 0, 15 },
        //                             { 0, 20 },
        //                             { 0, 25 },
        //                             { 0, 30 },
        //                             { 0, 35 },
        //                             { 0, 40 },
        //                             { 0, 45 } };
        //int[,] joints1 = new int[,] { { 0, 0 },
        //                             { 5, 0 },
        //                             { 10, 0 },
        //                             { 15, 0 },
        //                             { 20, 0 },
        //                             { 25, 0 },
        //                             { 30, 0 },
        //                             { 35, 0 },
        //                             { 40, 0 },
        //                             { 45, 0 }  };
        // Methods
        private void start()
        {
            //foreach (Pipe pipe in pipes) { pipe.start(); }
            guiClient.start();
            guiClient.write.WriteLine("Start");
            infoUpdateTimer.Enabled = true;
            //graphTimer.Enabled = true;
            this.Dispatcher.Invoke(() =>
            {
                StartButton.Content = "Stop";
                ExportButton.IsEnabled = true;
            });
        }
        private void stop()
        {
            infoUpdateTimer.Enabled = false;
            infoResetTimer.Enabled = true;
            infoUpdateTimer.Enabled = false;
            foreach (Pipe pipe in pipes) { pipe.stop(); }
            this.Dispatcher.Invoke(() =>
            {
                StartButton.IsEnabled = false;
                ExportButton.IsEnabled = false;
                StartButton.Content = "Start";
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
        private void parse(string line)
        {
            if (line != null) { 
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
                            joints[0, 0] = Convert.ToDouble(words[2]) * 100;
                            joints[0, 1] = Convert.ToDouble(words[3]) * 100;
                            break;
                        case "midspine":
                            MidSpineX.Text = words[2];
                            MidSpineY.Text = words[3];
                            MidSpineZ.Text = words[4];
                            joints[1, 0] = Convert.ToDouble(words[2]) * 100;
                            joints[1, 1] = Convert.ToDouble(words[3]) * 100;
                            break;
                        case "neck":
                            NeckX.Text = words[2];
                            NeckY.Text = words[3];
                            NeckZ.Text = words[4];
                            joints[2, 0] = Convert.ToDouble(words[2]) * 100;
                            joints[2, 1] = Convert.ToDouble(words[3]) * 100;
                            break;
                        case "shoulderleft":
                            ShoulderLeftX.Text = words[2];
                            ShoulderLeftY.Text = words[3];
                            ShoulderLeftZ.Text = words[4];
                            joints[3, 0] = Convert.ToDouble(words[2]) * 100;
                            joints[3, 1] = Convert.ToDouble(words[3]) * 100;
                            break;
                        case "elbowleft":
                            ElbowLeftX.Text = words[2];
                            ElbowLeftY.Text = words[3];
                            ElbowLeftZ.Text = words[4];
                            joints[4, 0] = Convert.ToDouble(words[2]) * 100;
                            joints[4, 1] = Convert.ToDouble(words[3]) * 100;
                            break;
                        case "wristleft":
                            WristLeftX.Text = words[2];
                            WristLeftY.Text = words[3];
                            WristLeftZ.Text = words[4];
                            joints[5, 0] = Convert.ToDouble(words[2]) * 100;
                            joints[5, 1] = Convert.ToDouble(words[3]) * 100;
                            break;
                        case "handleft":
                            HandLeftX.Text = words[2];
                            HandLeftY.Text = words[3];
                            HandLeftZ.Text = words[4];
                            joints[6, 0] = Convert.ToDouble(words[2]) * 100;
                            joints[6, 1] = Convert.ToDouble(words[3]) * 100;
                            break;
                        case "head":
                            HeadX.Text = words[2];
                            HeadY.Text = words[3];
                            HeadZ.Text = words[4];
                            joints[7, 0] = Convert.ToDouble(words[2]) * 100;
                            joints[7, 1] = Convert.ToDouble(words[3]) * 100;
                            break;
                        case "shoulderright":
                            ShoulderRightX.Text = words[2];
                            ShoulderRightY.Text = words[3];
                            ShoulderRightZ.Text = words[4];
                            joints[8, 0] = Convert.ToDouble(words[2]) * 100;
                            joints[8, 1] = Convert.ToDouble(words[3]) * 100;
                            break;
                        case "elbowright":
                            ElbowRightX.Text = words[2];
                            ElbowRightY.Text = words[3];
                            ElbowRightZ.Text = words[4];
                            joints[9, 0] = Convert.ToDouble(words[2]) * 100;
                            joints[9, 1] = Convert.ToDouble(words[3]) * 100;
                            break;
                        case "handright":
                            HandRightX.Text = words[2];
                            HandRightY.Text = words[3];
                            HandRightZ.Text = words[4];
                            joints[10, 0] = Convert.ToDouble(words[2]) * 100;
                            joints[10, 1] = Convert.ToDouble(words[3]) * 100;
                            break;
                        case "hipleft":
                            HipLeftX.Text = words[2];
                            HipLeftY.Text = words[3];
                            HipLeftZ.Text = words[4];
                            joints[11, 0] = Convert.ToDouble(words[2]) * 100;
                            joints[11, 1] = Convert.ToDouble(words[3]) * 100;
                            break;
                        case "kneeleft":
                            KneeLeftX.Text = words[2];
                            KneeLeftY.Text = words[3];
                            KneeLeftZ.Text = words[4];
                            joints[12, 0] = Convert.ToDouble(words[2]) * 100;
                            joints[12, 1] = Convert.ToDouble(words[3]) * 100;
                            break;
                        case "footleft":
                            FootLeftX.Text = words[2];
                            FootLeftY.Text = words[3];
                            FootLeftZ.Text = words[4];
                            joints[13, 0] = Convert.ToDouble(words[2]) * 100;
                            joints[13, 1] = Convert.ToDouble(words[3]) * 100;
                            break;
                        case "hipright":
                            HipRightX.Text = words[2];
                            HipRightY.Text = words[3];
                            HipRightZ.Text = words[4];
                            joints[14, 0] = Convert.ToDouble(words[2]) * 100;
                            joints[14, 1] = Convert.ToDouble(words[3]) * 100;
                            break;
                        case "kneeright":
                            KneeRightX.Text = words[2];
                            KneeRightY.Text = words[3];
                            KneeRightZ.Text = words[4];
                            joints[15, 0] = Convert.ToDouble(words[2]) * 100;
                            joints[15, 1] = Convert.ToDouble(words[3]) * 100;
                            break;
                        case "footright":
                            FootRightX.Text = words[2];
                            FootRightY.Text = words[3];
                            FootRightZ.Text = words[4];
                            joints[16, 0] = Convert.ToDouble(words[2]) * 100;
                            joints[16, 1] = Convert.ToDouble(words[3]) * 100;
                            break;
                        case "wristright":
                            WristRightX.Text = words[2];
                            WristRightY.Text = words[3];
                            WristRightZ.Text = words[4];
                            joints[17, 0] = Convert.ToDouble(words[2]) * 100;
                            joints[17, 1] = Convert.ToDouble(words[3]) * 100;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        //Events
        private void initalLoadTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                StartButton.IsEnabled = true;
            });
            
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
        void infoUpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                parse(guiClient.read.ReadLine());
            });
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            //Remove the previous ellipse from the paint canvas.
            for(int i = 0; i < bodypoints.Length; i++)
            {
                PaintCanvas.Children.Remove(bodypoints[i]);
                bodypoints[i] = CreateAnEllipse(10, 10);
                PaintCanvas.Children.Add(bodypoints[i]);
                Canvas.SetLeft(bodypoints[i], joints[i, 0]);
                Canvas.SetBottom(bodypoints[i], joints[i, 1]);

            }
            //>>>>>>PaintCanvas.Children.Remove(ellipse);
            //PaintCanvas.Children.Remove(ellipse2);
            //if (idx >= joints.GetLength(0))
            //{
            //    idx = 0;
            //}

            //Create ellipse with height and width
            //ellipse2 = CreateAnEllipse(20, 20);
            //>>>>>>>>>>>>ellipse = CreateAnEllipse(20, 20);

            //Add Ellipse to canvas 
            //>>>>>>>>>>>>>>>>PaintCanvas.Children.Add(ellipse);
            //PaintCanvas.Children.Add(ellipse2);

            //if (idx < joints.GetLength(0))
            //{
               //>>>>>>>>>>>>> Canvas.SetLeft(ellipse, joints[idx, 0]);
                //>>>>>>>>>>>>>Canvas.SetBottom(ellipse, joints[idx, 1]);
                // Canvas.SetLeft(ellipse2, joints1[idx, 0]);
                //Canvas.SetTop(ellipse2, joints1[idx, 1]);
          //  }
            //else
            //{
            //    idx = 0;
            //}

            //Increment the index
            //idx++;

        }

        // Gui Default Events
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
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
        /// <summary>
        /// Execute start up tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            //Initialize the timer class
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(.01); //Set the interval period here.
            timer.Tick += timer1_Tick;
            // loopCounter = 10;
            timer.Start();
            //  graphTimer.Elapsed += new ElapsedEventHandler(graphTimer_Elapsed);
            infoUpdateTimer.Elapsed += new ElapsedEventHandler(infoUpdateTimer_Elapsed);
            infoResetTimer.Elapsed += new ElapsedEventHandler(infoResetTimer_Elapsed);
            initalLoadTimer.Elapsed += new ElapsedEventHandler(initalLoadTimer_Elapsed);
        }
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void MenuItem_board_Click_1(object sender, RoutedEventArgs e)
        {
            BoardWindow popup = new BoardWindow();
            popup.ShowDialog();
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
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Connect_Click(object sender, RoutedEventArgs e)
        {

        }
        public Ellipse CreateAnEllipse(int height, int width)
        {
            SolidColorBrush fillBrush = new SolidColorBrush() { Color = Colors.Red };
            SolidColorBrush borderBrush = new SolidColorBrush() { Color = Colors.Black };

            return new Ellipse()
            {
                Height = height,
                Width = width,
                StrokeThickness = 1,
                Stroke = borderBrush,
                Fill = fillBrush
            };
        }
    }
}

