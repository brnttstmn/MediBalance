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
        MSBand2 band = new MSBand2();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Connect_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate an instance of the MSBand2, then call working function
            // **Note: button click method must be "async" and "await" is needed prior to function call
            //          WILL LOCK UP OTHERWISE


            int a = 5;
            //int n = 5;
            string Hb_arr = "Hb: ";
            textBox.Text = "hello world";
            string host = "10.109.97.195";
            string port = "8001";
            string serverresponse;
            //for (int i = 0; i < n; i++)

            try
            {
                await band.everything(a, connection_text);
                Hb_arr += band.Hb;
            }
            catch (Exception) { connection_text.Text = "Please only enter an integer"; }

            Hb_arr += band.Hb;
            textBox.Text = Hb_arr;
            Tcp_Client clnt = new Tcp_Client();
            serverresponse = await clnt.sendit(host, port, Hb_arr);
            clnt.close();

        }
    }
}
