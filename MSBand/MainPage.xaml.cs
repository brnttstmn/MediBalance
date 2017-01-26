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
using System.Collections;


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


        /// <Main Method>
        /// This will be the main method of this module.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            int time;
            var preload = new bool[4] {false,false,false,false};
            var control = new BitArray(preload);           
            var c = new Dictionary<string, int>() { {"hr", 0 }, {"gsr",1}, {"ls",2},{"debug",3}};

            // Only run if provide time is an integer
            if (Int32.TryParse(textBox.Text, out time))
            {
                // Check for Config
                if (debug_checkbox.IsChecked == true) { control[c["debug"]] = true; }
                if (hr_checkBox.IsChecked == true) { control[c["hr"]] = true; }
                if (gsr_checkBox.IsChecked == true) { control[c["gsr"]] = true; }
                if (ls_checkBox.IsChecked == true) { control[c["ls"]] = true; }

                if (control[c["debug"]]) { sim_band(time, control, c); }
                else { run_band(time, control, c, connection_text); }

            }
            // Inform user that function requires integers
            else { connection_text.Text = "Please only enter intergers."; }
        }

        /// <Run Band>
        /// This method runs the band for the amount of time given
        /// </summary>
        /// <param name="time"></param>
        /// <param name="connection_text"></param>
        private async void run_band(int time, BitArray control, Dictionary<string, int> map, TextBlock connection_text) {

            // Declare Objects
            MSBand2 band = new MSBand2();
            var samples = new List<string>();
            int stat;

            connection_text.Text = "Connecting...";
            stat = await band.everything(time, samples, control, map, connection_text);

            if (stat == 0) { connection_text.Text += string.Format("\nFinished Sampling"); }
            if (stat==-1){ connection_text.Text = "Microsoft Band cannot be found. Check Connection"; }
            if (stat == -2) { connection_text.Text = "Access to the heart rate sensor is denied."; }
            

        }

        /// <DEBUG: Simulated Band>
        /// This method is a simulation of the band
        /// </summary>
        /// <param name="time"></param>
        private async void sim_band(int time, BitArray control, Dictionary<string, int> map)
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

        private void checkBox_Checked_hr(object sender, RoutedEventArgs e)
        {

        }

        private void checkBox_Checked_gsr(object sender, RoutedEventArgs e)
        {

        }

        private void checkBox_Checked_ls(object sender, RoutedEventArgs e)
        {

        }

        private void checkBox_Checked_debug(object sender, RoutedEventArgs e)
        {

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