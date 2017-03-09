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
        NamedPipeClientStream guiClient;
        StreamReader read;       

        System.Timers.Timer infoUpdateTimer = new System.Timers.Timer() { Interval = 5, Enabled = false };

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
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
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
            StartButton.IsEnabled = false;
            Task.Run(() =>
            {
                guiClient = new NamedPipeClientStream(".", "interface", PipeDirection.InOut);
                guiClient.Connect();
                read = new StreamReader(guiClient);
                infoUpdateTimer.Enabled = true;
                System.Threading.Thread.Sleep(10000);
                guiClient.Dispose();
                infoUpdateTimer.Enabled = false;
                this.Dispatcher.Invoke(() =>
                {
                    StartButton.IsEnabled = true;
                });                
            });
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {

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
            var line = read.ReadLine();
            this.Dispatcher.Invoke(() =>
            {
                if (line != null) { parse(line); }                
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
                // Brent you didn't finish putting in all of the fields!!!!
                    //case "elbowright":
                    //    elbo.Text = words[2];
                    //    MidSpineY.Text = words[3];
                    //    MidSpineZ.Text = words[4];
                    //    break;
                    //case "handright":
                    //    NeckX.Text = words[2];
                    //    NeckY.Text = words[3];
                    //    NeckZ.Text = words[4];
                    //    break;
                    //case "hipleft":
                    //    SpineBaseX.Text = words[2];
                    //    SpineBaseY.Text = words[3];
                    //    SpineBaseZ.Text = words[4];
                    //    break;
                    //case "kneeleft":
                    //    SpineBaseX.Text = words[2];
                    //    SpineBaseY.Text = words[3];
                    //    SpineBaseZ.Text = words[4];
                    //    break;
                    //case "footleft":
                    //    MidSpineX.Text = words[2];
                    //    MidSpineY.Text = words[3];
                    //    MidSpineZ.Text = words[4];
                    //    break;
                    //case "hipright":
                    //    NeckX.Text = words[2];
                    //    NeckY.Text = words[3];
                    //    NeckZ.Text = words[4];
                    //    break;
                    //case "kneeright":
                    //    HeadX.Text = words[2];
                    //    HeadY.Text = words[3];
                    //    HeadZ.Text = words[4];
                    //    break;
                    //case "footright":
                    //    HeadX.Text = words[2];
                    //    HeadY.Text = words[3];
                    //    HeadZ.Text = words[4];
                    //    break;
                    default:
                            break;
                    }
                }
            }
        }

    }

