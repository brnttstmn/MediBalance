using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.IO.Pipes;

namespace Tunnel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Start_Click(object sender, EventArgs e)
        {
            NamedPipeServerStream kServer = new NamedPipeServerStream("tunnel", PipeDirection.InOut);
            StreamWriter sw = null;
            StreamReader sr = null;

            data.AppendText("Waiting on Pipe Connection...");
            //data.AppendText(Environment.NewLine);


            // Connect to Pipe
            //kClient.Connect();
            kServer.WaitForConnection();
            sr = new StreamReader(kServer);
            sw = new StreamWriter(kServer) { AutoFlush = true };

            data.AppendText("The Pipe is Open");
            data.AppendText(Environment.NewLine);



            try
            {
                IPAddress ipAd = IPAddress.Parse("10.109.65.51");
                // use local m/c IP address, and 
                // use the same in the client

                /* Initializes the Listener */
                TcpListener myList = new TcpListener(ipAd, 8001);

                /* Start Listeneting at the specified port */
                myList.Start();

                data.AppendText("The server is running at port 8001...");
                //data.AppendText(Environment.NewLine);


                data.AppendText("The local End point is  :" + myList.LocalEndpoint);
                //data.AppendText(Environment.NewLine);

                data.AppendText("...Waiting for a connection...");
                //data.AppendText(Environment.NewLine);


                Socket s = await myList.AcceptSocketAsync();
                data.AppendText("Connection accepted from " + s.RemoteEndPoint);
                data.AppendText(Environment.NewLine);

                byte[] b = new byte[100];
                Thread.Sleep(10000);

                while (true)
                {
                    Thread.Sleep(3000);
                    int k = s.Receive(b);
                    if (k > 0)
                    {
                        data.AppendText("Received...");
                        //data.AppendText(Environment.NewLine);
                    }
                    else
                    {
                        data.AppendText("Error no bytes");
                        data.AppendText(Environment.NewLine);
                    }

                    string result = Encoding.UTF8.GetString(b);

                    data.AppendText(result);
                    data.AppendText(Environment.NewLine);

                    sw.WriteLine(result);

                    //Encoding utf = Encoding.UTF8;
                    //s.Send(utf.GetBytes(msg));
                    data.AppendText("Data Passed to Backend");
                    data.AppendText(Environment.NewLine);
                }
                /* clean up */
                s.Close();
                myList.Stop();



            }
            catch (SocketException y)
            {
                data.AppendText("Error..." + y.StackTrace);
                data.AppendText(Environment.NewLine);
            }

        }

        private void data_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
