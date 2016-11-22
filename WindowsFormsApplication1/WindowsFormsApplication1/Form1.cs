using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        
        //static string stats = "hello world";
        //static string[] stats_arr = new string[50];
        //String.Join("hello world",stats_arr)

        public Form1()
        {
            InitializeComponent();
        }

        private async void Listen_Click(object sender, EventArgs e)
        {
            Data.Text = "hello world";

            try
            {
                IPAddress ipAd = IPAddress.Parse("10.109.97.195");
                // use local m/c IP address, and 
                // use the same in the client

                /* Initializes the Listener */
                TcpListener myList = new TcpListener(ipAd, 8001);

                /* Start Listeneting at the specified port */
                myList.Start();

                Data.AppendText("The server is running at port 8001...");
                Data.AppendText(Environment.NewLine);


                Data.AppendText("The local End point is  :" + myList.LocalEndpoint);
                Data.AppendText(Environment.NewLine);

                Data.AppendText("Waiting for a connection.....");
                Data.AppendText(Environment.NewLine);


                Socket s = await myList.AcceptSocketAsync();
                Data.AppendText("Connection accepted from " + s.RemoteEndPoint);
                Data.AppendText(Environment.NewLine);

                byte[] b = new byte[100];
                int k = s.Receive(b);
                Data.AppendText("Received");
                Data.AppendText(Environment.NewLine);


                string result = Encoding.UTF8.GetString(b);

                Data.AppendText(result);
                Data.AppendText(Environment.NewLine);

                Encoding utf = Encoding.UTF8;
                s.Send(utf.GetBytes("The string was recieved by the server."));
                Data.AppendText("Sent Acknowledgement");
                Data.AppendText(Environment.NewLine);
                /* clean up */
                s.Close();
                myList.Stop();



            }
            catch (SocketException y)
            {
                Data.AppendText("Error..." + y.StackTrace);
                Data.AppendText(Environment.NewLine);
            } 
        }
    }
}
