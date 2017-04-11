using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Timers;
using System.Windows.Shapes;
using SharedLibraries;

namespace FrontEndUIRedux
{
    public partial class MainWindow : Window
    {
        // Create Pipe Objects
        static Pipe guiClient = new Pipe("interface", true);
        static Pipe guiCommands = new Pipe("frominterface", true);
        Pipe[] pipes = { guiClient, guiCommands };
        static string status = "";
        static short time = 30;
        static short timeElapsed = 0;

        // Set Timers
        Timer infoUpdateTimer = new Timer() { Interval = 1, Enabled = false };
        Timer timeTimer = new Timer() { Interval = 100, Enabled = false };
        Timer infoResetTimer = new Timer() { Interval = 500, Enabled = false };
        Timer statusTimer = new Timer() { Interval = 1000, Enabled = true };
        Timer initalLoadTimer = new Timer() { Interval = 1500, Enabled = true };
        Timer stopLoggingTimer = new Timer() { Interval = time * 1000, Enabled = false };
        Timer graphTimer = new Timer() { Interval = 1, Enabled = false };

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
            log();
        }

        //Ellipse initialization
        Ellipse[] bodypoints = new Ellipse[18];
        Ellipse COB = new Ellipse();
        double[,] COBpoint = new double[1, 2];
        double[,] joints = new double[18, 2];

        // Methods
        private void start()
        {
            graphTimer.Enabled = true;
            status = "running";
            Pipe.connectPipes(pipes);
            infoUpdateTimer.Enabled = true;
            this.Dispatcher.Invoke(() =>
            {
                StartButton.Content = "Stop";
                ExportButton.IsEnabled = true;
            });
            guiCommands.read.ReadLine();
        }
        private void stop()
        {
            graphTimer.Enabled = false;
            status = "";
            infoUpdateTimer.Enabled = false;
            infoResetTimer.Enabled = true;
            Pipe.disconnectPipes(pipes);
            this.Dispatcher.Invoke(() =>
            {
                StartButton.IsEnabled = false;
                ExportButton.IsEnabled = false;
                StartButton.Content = "Start";
            });
        }
        private void log()
        {
            status = "logging";
            Dictionary<string, bool> stances = new Dictionary<string, bool>(){
                { "SingleLegStanceRadio", SingleLegStanceRadio.IsChecked == true },
                { "DoubleLegStanceRadio", DoubleLegStanceRadio.IsChecked == true },
                { "TandemLegStanceRadio", TandemLegStanceRadio.IsChecked == true }};

            ExportButton.IsEnabled = false;
            ExportButton.Content = "Collecting Data...";

            try { time = Int16.Parse(Seconds.Text); }
            catch (Exception) { }
            stopLoggingTimer = new Timer() { Interval = time * 1000, Enabled = false };
            stopLoggingTimer.Elapsed += new ElapsedEventHandler(stopLoggingTimer_Elapsed);
            stopLoggingTimer.Enabled = true;
            
            foreach (KeyValuePair<string, bool> stance in stances)
            {
                if (stance.Value) { guiCommands.write.WriteLine(stance.Key); }
            }
            
        }

        private void reset()
        {
            foreach (Ellipse point in bodypoints)
            {
                PaintCanvas.Children.Remove(point);
            }
            string blank = "";
            HeartRateTextBlock.Text = blank;
            GSRTextBlock.Text = blank;
            RWeight.Text = blank;
            TLeft.Text = blank;
            TRight.Text = blank;
            BRight.Text = blank;
            BLeft.Text = blank;
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
            {"KinectEnvironment","BalanceBoard","Backend","Bridge","MediBalance","BackEnd.EXE"
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
            try
            {
                if (line != null)
                {
                    char[] delim = { ',', ';' };
                    string[] words = line.Split(delim);
                    if (words.Length > 2)
                    {
                        switch (words[1])
                        {
                            case "Heartrate":
                                HeartRateTextBlock.Text = words[2];
                                break;
                            case "gsr":
                                GSRTextBlock.Text = words[2];
                                break;
                            case "RWeight":
                                RWeight.Text = words[2];
                                break;
                            case "TopLeft":
                                TLeft.Text = words[2];

                                break;
                            case "TopRight":
                                TRight.Text = words[2];
                                break;
                            case "BottomRight":
                                BRight.Text = words[2];
                                break;
                            case "BottomLeft":
                                BLeft.Text = words[2];
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
            catch (FormatException){ }
        }
        //private void BalancePlot(EnvironmentVariableTarget TL)
        //{
        //    float naCorners = 0f;
        //    var owTopLeft = ;
        //    var owTopRight = ;
        //    var owBottomLeft = ;
        //    var owBottomRight = ;


        //    var owrPercentage = 100 / (owTopLeft + owTopRight + owBottomLeft + owBottomRight);
        //    var owrTopLeft = owrPercentage * owTopLeft;
        //    var owrTopRight = owrPercentage * owTopRight;
        //    var owrBottomLeft = owrPercentage * owBottomLeft;
        //    var owrBottomRight = owrPercentage * owBottomRight;

        //    var brX = owrBottomRight + owrTopRight;
        //    var brY = owrBottomRight + owrBottomLeft;
        //}

        //Events
        private void timeTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                RealTimeClock.Text = "Current Time: " + DateTime.Now.ToString("HH:mm:ss");
            });
        }
        private void statusTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                switch (status)
                {
                    case "running":
                        if (Status_Bar.Text.Equals("") || Status_Bar.Text.Equals("Running...")) { Status_Bar.Text = "Running"; }
                        else if (Status_Bar.Text.Equals("Running")) { Status_Bar.Text = "Running."; }
                        else if (Status_Bar.Text.Equals("Running.")) { Status_Bar.Text = "Running.."; }
                        else if (Status_Bar.Text.Equals("Running..")) { Status_Bar.Text = "Running..."; }
                        break;
                    case "logging":
                        if (Status_Bar.Text.Contains("Running")|| Status_Bar.Text.Contains("Logging...")) { Status_Bar.Text = "Logging\t\t" + (time - timeElapsed++).ToString(); }
                        else if (Status_Bar.Text.Contains("Logging..")) { Status_Bar.Text = "Logging...\t" + (time - timeElapsed++).ToString(); }
                        else if (Status_Bar.Text.Contains("Logging.")) { Status_Bar.Text = "Logging..\t" + (time - timeElapsed++).ToString(); }
                        else if (Status_Bar.Text.Contains("Logging")) { Status_Bar.Text = "Logging.\t\t" + (time - timeElapsed++).ToString(); }
                        break;
                    case "":
                    default:
                        timeElapsed = 0;
                        Status_Bar.Text = "";
                        break;
                }
            });
        }
        private void stopLoggingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            stopLoggingTimer.Enabled = false;
            stopLoggingTimer.Elapsed -= new ElapsedEventHandler(stopLoggingTimer_Elapsed);
            stop();
        }
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
                System.Threading.Thread.Sleep(500);
                StartButton.IsEnabled = true;
            });
        }
        void infoUpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                infoUpdateTimer.Enabled = false;
                var line = guiClient.read.ReadLine();
                this.Dispatcher.Invoke(() =>
                {
                    parse(line);
                });
                infoUpdateTimer.Enabled = true;
            }
            catch (ObjectDisposedException) {  }
        }
        private void graphTimer_Elapsed(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                for (int i = 0; i < bodypoints.Length; i++)
                {
                    try
                    {
                        PaintCanvas.Children.Remove(bodypoints[i]);
                        bodypoints[i] = CreateAnEllipse(10, 10);
                        PaintCanvas.Children.Add(bodypoints[i]);
                        Canvas.SetLeft(bodypoints[i], joints[i, 0]);
                        Canvas.SetBottom(bodypoints[i], joints[i, 1]);
                    }
                    catch (Exception) { }
                    
                }

            //PaintCanvas.Children.Remove(COB);
            //COB = CreateAnEllipse(10, 10);
            //PaintCanvas.Children.Add(COB);
            //Canvas.SetLeft(COB, COBpoint[0,0]);
            //Canvas.SetBottom(COB, COBpoint[0, 1]);
            });

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
            graphTimer.Elapsed += new ElapsedEventHandler(graphTimer_Elapsed);
            timeTimer.Elapsed += new ElapsedEventHandler(timeTimer_Elapsed);
            infoUpdateTimer.Elapsed += new ElapsedEventHandler(infoUpdateTimer_Elapsed);
            infoResetTimer.Elapsed += new ElapsedEventHandler(infoResetTimer_Elapsed);
            initalLoadTimer.Elapsed += new ElapsedEventHandler(initalLoadTimer_Elapsed);
            statusTimer.Elapsed += new ElapsedEventHandler(statusTimer_Elapsed);
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

