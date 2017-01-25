using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Band;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MediBalance
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        /// <Main Method>
        /// This will be the main method of this module.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            int time;

            // Only run if provide time is an integer
            if (Int32.TryParse(textBox.Text, out time))
            {
                // Check if Debug box is checked
                if (debug_checkbox.IsChecked == false) { run_band(time, connection_text); }
                else { sim_band(time); }
                
            }
            // Inform user that function requires integers
            else { connection_text.Text = "Please only enter intergers."; }
        }

        /// <Run Band>
        /// This method runs the band for the amount of time given
        /// </summary>
        /// <param name="time"></param>
        /// <param name="connection_text"></param>
        private async void run_band(int time, TextBlock connection_text) {

            // Declar Objects
            MSBand2 band = new MSBand2();
            var samples = new List<string>();


            await band.everything(time, connection_text, samples);
        }

        /// <DEBUG: Simulated Band>
        /// This method is a simulation of the band
        /// </summary>
        /// <param name="time"></param>
        private async void sim_band(int time)
        {
            // Declare Objects
            var test = new TestVectors.TestBand();
            var hr = new List<int>();
            var samples = new List<string>();

            // User feedback
            connection_text.Text = "Simulated HeartRate:\n";

            // Random Number Generator(s)
            await test.heartRate(time, hr);

            // Simulate Wait Time
            for (int i = 0; i < hr.Count; i++) {
                await Task.Delay(1000);
                samples.Add(hr[i].ToString() + ": " + DateTime.Now.ToString("hh:mm:ss:fff tt"));
                connection_text.Text += samples[i]+'\n';
            }
        }
    }
}
//int a = 5;
////int n = 5;
//string Hb_arr = "Hb: ";
//textBox.Text = "hello world";
//string host = "10.109.97.195";
//string port = "8001";
//string serverresponse;
////for (int i = 0; i < n; i++)
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//Hb_arr += band.Hb;
//textBox.Text = Hb_arr;
//Tcp_Client clnt = new Tcp_Client();
//serverresponse = await clnt.sendit(host, port, Hb_arr);
//clnt.close();