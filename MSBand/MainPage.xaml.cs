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
        Tcp_Client tcpClient = new Tcp_Client();

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

            // Only run if provide time is an integer
            if (Int32.TryParse(textBox.Text, out time))
            {
                // Check for Config
                run_band(time);
            }
            // Inform user that function requires integers
            else { connection_text.Text = "Please only enter intergers."; }
        }

        /// <Run Band>
        /// This method runs the band for the amount of time given
        /// </summary>
        /// <param name="time"></param>
        /// <param name="connection_text"></param>
        private async void run_band(int time = 999)
        {
            tcpClient.create_socket();
            tcpClient.connect(Tcp_Client.GetLocalIp());

            // Declare Objects
            MSBand2 band = new MSBand2();
            var samples = new List<string>();
            int stat;
            string ipadd = IP_Box.Text;
            connection_text.Text = "Connecting...";
            stat = await band.everything(time, connection_text, tcpClient);

            if (stat == 0) { connection_text.Text += string.Format("\nFinished Sampling"); }
            if (stat == -1) { connection_text.Text = "Microsoft Band cannot be found. Check Connection"; }
            if (stat == -2) { connection_text.Text = "Access to the heart rate sensor is denied."; }
        }

        private void IP_Box_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}