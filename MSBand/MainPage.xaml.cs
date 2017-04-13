using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections;
//using Windows.Task;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MediBalance
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class MainPage : Page
    {
        string timeFormat = "HH:mm:ss:fff";
        Tcp_Client tcpClient = new Tcp_Client();

        public MainPage()
        {
            this.InitializeComponent();
            tcpClient.create_socket();
            tcpClient.connect(Tcp_Client.GetLocalIp());
        }

        public async void listen()
        {
            connection_text.Text += "listening";
            string mess = await tcpClient.read();
            connection_text.Text += mess;
        }

        private void Start()
        {

            int time;
            var preload = new bool[4] { false, false, false, false };
            var control = new BitArray(preload);
            var c = new Dictionary<string, int>() { { "hr", 0 }, { "gsr", 1 }, { "ls", 2 }, { "debug", 3 } };

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

        /// <Main Method>
        /// This will be the main method of this module.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            int time;
            var preload = new bool[4] { false, false, false, false };
            var control = new BitArray(preload);
            var c = new Dictionary<string, int>() { { "hr", 0 }, { "gsr", 1 }, { "ls", 2 }, { "debug", 3 } };

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
        private async void run_band(int time, BitArray control, Dictionary<string, int> map, TextBlock connection_text)
        {

            // Declare Objects
            MSBand2 band = new MSBand2();
            var samples = new List<string>();
            int stat;
            string ipadd = IP_Box.Text;
            connection_text.Text = "Connecting...";
            stat = await band.everything(time, samples, control, map, connection_text, tcpClient);

            if (stat == 0) { connection_text.Text += string.Format("\nFinished Sampling"); }
            if (stat == -1) { connection_text.Text = "Microsoft Band cannot be found. Check Connection"; }
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

            for (int i = 0; i < hr.Count; i++)
            {
                await Task.Delay(1000);
                samples.Add(string.Format("{0},Heartrate,{1};", DateTime.Now.ToString(timeFormat), hr[i].ToString()));
                connection_text.Text += samples[i] + '\n';
                await tcpClient.send(samples[i]);
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

        private void IP_Box_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}