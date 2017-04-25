using SharedLibraries;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FrontEndUIRedux
{
    public partial class MainWindow : Window
    {
        // Create Pipe Objects
        static Pipe guiClient = new Pipe("interface", true);
        static Pipe guiCommands = new Pipe("frominterface", true);
        Pipe[] pipes = { guiClient, guiCommands };

        // GUI Status Variables
        static string status = "";
        static short time = 30;
        static short timeElapsed = 0;

        // Ellipse initialization
        Ellipse[] bodypoints = new Ellipse[18];
        Ellipse COB = new Ellipse();
        double[] COBpoint = { 0,0 };
        double[,] joints = new double[18, 2];

        // Set Timers
        Timer initalLoadTimer = new Timer() { Interval = 1500, Enabled = true };
        Timer statusTimer = new Timer() { Interval = 100, Enabled = true };
        Timer infoUpdateTimer = new Timer() { Interval = 1, Enabled = false };
        Timer infoResetTimer = new Timer() { Interval = 500, Enabled = false };
        Timer stopLoggingTimer = new Timer() { Interval = time * 1000, Enabled = false };

        // Button Presses
        public void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (status.Contains("running")|| status.Contains("logging"))
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

        // Methods
        private void start()
        {
            programhandler.stopPrograms();
            programhandler.runPrograms();
            Dispatcher.Invoke(() =>
            {
                StartButton.Content = "Stop";
                ExportButton.IsEnabled = true;
            });
            status = "running";
            Task.Run(() =>
            {
                Pipe.connectPipes(pipes);
                infoUpdateTimer.Enabled = true;
            });           
        }
        private void stop()
        {
            status = "";
            infoUpdateTimer.Enabled = false;
            infoResetTimer.Enabled = true;
            Pipe.disconnectPipes(pipes);
            Dispatcher.Invoke(() =>
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
                kinect.Children.Remove(point);
            }
            BalanceCanvas.Children.Remove(COB);
            string blank = "";
            HeartRateTextBlock.Text = blank;
            GSRTextBlock.Text = blank;
            RWeight.Text = blank;
            TLeft.Text = blank;
            TRight.Text = blank;
            BRight.Text = blank;
            BLeft.Text = blank;
        }
        private void parse(string lines)
        {
            try
            {
                if (lines != null)
                {
                    char[] split = { ';' };
                    char[] delim = { ',', ';' };
                    foreach (string line in lines.Split(split))
                    {
                        string[] words = line.Split(delim);
                        if (words.Length > 2)
                        {
                            Console.WriteLine(words[1] + " : " + words[2]);
                            switch (words[1])
                            {
                                case "Heartrate":
                                    HeartRateTextBlock.Text = Convert.ToDouble(words[2]).ToString();
                                    break;
                                case "gsr":
                                    GSRTextBlock.Text = Convert.ToDouble(words[2]).ToString();
                                    break;
                                case "RWeight":
                                    RWeight.Text = Convert.ToDouble(words[2]).ToString();
                                    break;
                                case "TopLeft":
                                    TLeft.Text = Convert.ToDouble(words[2]).ToString();
                                    break;
                                case "TopRight":
                                    TRight.Text = Convert.ToDouble(words[2]).ToString();
                                    break;
                                case "BottomRight":
                                    BRight.Text = Convert.ToDouble(words[2]).ToString();
                                    break;
                                case "BottomLeft":
                                    BLeft.Text = Convert.ToDouble(words[2]).ToString();
                                    break;
                                case "spinebase":
                                    joints[0, 0] = Convert.ToDouble(words[2]) * 100;
                                    joints[0, 1] = Convert.ToDouble(words[3]) * 100;
                                    break;
                                case "midspine":
                                    joints[1, 0] = Convert.ToDouble(words[2]) * 100;
                                    joints[1, 1] = Convert.ToDouble(words[3]) * 100;
                                    break;
                                case "neck":
                                    joints[2, 0] = Convert.ToDouble(words[2]) * 100;
                                    joints[2, 1] = Convert.ToDouble(words[3]) * 100;
                                    break;
                                case "shoulderleft":
                                    joints[3, 0] = Convert.ToDouble(words[2]) * 100;
                                    joints[3, 1] = Convert.ToDouble(words[3]) * 100;
                                    break;
                                case "elbowleft":
                                    joints[4, 0] = Convert.ToDouble(words[2]) * 100;
                                    joints[4, 1] = Convert.ToDouble(words[3]) * 100;
                                    break;
                                case "wristleft":
                                    joints[5, 0] = Convert.ToDouble(words[2]) * 100;
                                    joints[5, 1] = Convert.ToDouble(words[3]) * 100;
                                    break;
                                case "handleft":
                                    joints[6, 0] = Convert.ToDouble(words[2]) * 100;
                                    joints[6, 1] = Convert.ToDouble(words[3]) * 100;
                                    break;
                                case "head":
                                    joints[7, 0] = Convert.ToDouble(words[2]) * 100;
                                    joints[7, 1] = Convert.ToDouble(words[3]) * 100;
                                    break;
                                case "shoulderright":
                                    joints[8, 0] = Convert.ToDouble(words[2]) * 100;
                                    joints[8, 1] = Convert.ToDouble(words[3]) * 100;
                                    break;
                                case "elbowright":
                                    joints[9, 0] = Convert.ToDouble(words[2]) * 100;
                                    joints[9, 1] = Convert.ToDouble(words[3]) * 100;
                                    break;
                                case "handright":
                                    joints[10, 0] = Convert.ToDouble(words[2]) * 100;
                                    joints[10, 1] = Convert.ToDouble(words[3]) * 100;
                                    break;
                                case "hipleft":
                                    joints[11, 0] = Convert.ToDouble(words[2]) * 100;
                                    joints[11, 1] = Convert.ToDouble(words[3]) * 100;
                                    break;
                                case "kneeleft":
                                    joints[12, 0] = Convert.ToDouble(words[2]) * 100;
                                    joints[12, 1] = Convert.ToDouble(words[3]) * 100;
                                    break;
                                case "footleft":
                                    joints[13, 0] = Convert.ToDouble(words[2]) * 100;
                                    joints[13, 1] = Convert.ToDouble(words[3]) * 100;
                                    break;
                                case "hipright":
                                    joints[14, 0] = Convert.ToDouble(words[2]) * 100;
                                    joints[14, 1] = Convert.ToDouble(words[3]) * 100;
                                    break;
                                case "kneeright":
                                    joints[15, 0] = Convert.ToDouble(words[2]) * 100;
                                    joints[15, 1] = Convert.ToDouble(words[3]) * 100;
                                    break;
                                case "footright":
                                    joints[16, 0] = Convert.ToDouble(words[2]) * 100;
                                    joints[16, 1] = Convert.ToDouble(words[3]) * 100;
                                    break;
                                case "wristright":
                                    joints[17, 0] = Convert.ToDouble(words[2]) * 100;
                                    joints[17, 1] = Convert.ToDouble(words[3]) * 100;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                }
            }
            catch (FormatException) { }
            catch (IndexOutOfRangeException) { }
        }
        private void BalancePlot()
        {
            // Dimension of Board (in mm)
            var x = 280;
            var y = 180;

            var owTopLeft = Convert.ToDouble(TLeft.Text);
            var owTopRight = Convert.ToDouble(TRight.Text);
            var owBottomLeft = Convert.ToDouble(BLeft.Text);
            var owBottomRight = Convert.ToDouble(BRight.Text);
            
            var owrPercentage = 100 / (owTopLeft + owTopRight + owBottomLeft + owBottomRight);
            var owrTopLeft = owrPercentage * owTopLeft;
            var owrTopRight = owrPercentage * owTopRight;
            var owrBottomLeft = owrPercentage * owBottomLeft;
            var owrBottomRight = owrPercentage * owBottomRight;
            COBpoint[0] = (x / 2) * ((owTopRight+owBottomRight) - (owTopLeft + owBottomLeft)) /(owTopLeft+owTopRight+owBottomLeft+owBottomRight);
            COBpoint[1] = (y / 2) * ((owTopRight + owTopLeft)-(owBottomLeft+owBottomRight)) / (owTopLeft + owTopRight + owBottomLeft + owBottomRight);
        }

        //Events
        private void statusTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                RealTimeClock.Text = DateTime.Now.ToString("HH:mm:ss");
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
            Dispatcher.Invoke(() =>
            {
                StartButton.IsEnabled = true;
            });
            
        }
        private void infoResetTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            infoResetTimer.Enabled = false;
            Dispatcher.Invoke(() =>
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
                Dispatcher.Invoke(() =>
                {
                    parse(line);
                    for (int i = 0; i < bodypoints.Length; i++)
                    {
                        try
                        {
                            kinect.Children.Remove(bodypoints[i]);
                            bodypoints[i] = CreateAnEllipse(8, 8);
                            kinect.Children.Add(bodypoints[i]);
                            Canvas.SetLeft(bodypoints[i], joints[i, 0]);
                            Canvas.SetTop(bodypoints[i], joints[i, 1]);
                        }
                        catch (Exception) { }

                    }
                    try
                    {
                        BalancePlot();
                    }
                    catch (Exception) { }
                    BalanceCanvas.Children.Remove(COB);
                    COB = CreateAnEllipse(10, 10);
                    BalanceCanvas.Children.Add(COB);
                    Canvas.SetLeft(COB, COBpoint[0]);
                    Canvas.SetTop(COB, COBpoint[1]);
                });
                infoUpdateTimer.Enabled = true;
            }
            catch (ObjectDisposedException) { }
            catch (TaskCanceledException) { }
        }

        // Start and Close Events
        /// <summary>
        /// Execute start up tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Initialize the timer class
            statusTimer.Elapsed += new ElapsedEventHandler(statusTimer_Elapsed);
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
            programhandler.stopPrograms();
        }

        // Logging Options
        private void SingleLegRadio_Checked(object sender, RoutedEventArgs e)
        {

        }
        private void DoubleLegRadio_Checked(object sender, RoutedEventArgs e)
        {

        }
        private void TandemLegRadio_Checked(object sender, RoutedEventArgs e)
        {

        }

        // Menu Items
        private void Help_Click(object sender, RoutedEventArgs e)
        {

        }
        private void MenuItem_board_Click_1(object sender, RoutedEventArgs e)
        {
            BoardWindow popup = new BoardWindow();
            popup.ShowDialog();
        }
        private void MenuItem_band_Click_1(object sender, RoutedEventArgs e)
        {
            BandWindow popup = new BandWindow();
            popup.ShowDialog();
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

        // Default Gui Instantiation
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}

