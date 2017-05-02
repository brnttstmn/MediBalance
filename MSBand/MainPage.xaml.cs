using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MediBalance
{

    public sealed partial class MainPage : Page
    {
        Tcp_Client tcpClient = new Tcp_Client();

        public MainPage()
        {
            this.InitializeComponent();
        }
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            while (true)
            {
                await run_band();
            }
        }

            /// <Main Method>
            /// This will be the main method of this module.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private async void Connect_Click(object sender, RoutedEventArgs e)
        {
                await run_band();
        }

        private async Task run_band(int time = 999)
        {
            // Declare Objects
            MSBand2 band = new MSBand2();
            bool connect = false;

            connection_text.Text = "Connecting TCP...";
            do {
                await Task.Delay(1);
                tcpClient.create_socket();
                connect = await tcpClient.connect(Tcp_Client.GetLocalIp());
               }
            while (!connect);
            connection_text.Text = "TCP Connected.\nConnecting Band...";

            await band.everything(time, connection_text, tcpClient);
            connection_text.Text = "done";
        }
    }
}