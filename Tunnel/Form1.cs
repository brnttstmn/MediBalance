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
        NamedPipeServerStream kServer = new NamedPipeServerStream("tunnel", PipeDirection.InOut);
        StreamWriter sw = null;
        StreamReader sr = null;
        IPAddress ipAd = null;
        /* Creates the TCP Listener and Socket */
        TcpListener myList = null;
        Socket s = null;
        //Create receive buffer for TCP connection
        static int buffer_len = 28;
        byte[] b = new byte[buffer_len];
        string readresult = null;

        public Form1()
        {
            InitializeComponent();
            //connectPipe();
            //connectTcp();
            //readTcp();
            
            run();



            
            //data.AppendText("hello world");
            //Thread.Sleep(2000);
            //bridge();
            //Starttunnel();
        }

        private void connecteverything()
        {
            connectPipe();
            connectTcp();
        }

        private void run()
        {
            connecteverything();


            //while (true) readTcp();
        }


        private async void connectTcp()
        {
            bool connection = false;

            while (!connection)
            {
                try
                {
                    string ipadd = GetLocalIPAddress();
                    data.AppendText(ipadd);
                    ipAd = IPAddress.Parse(ipadd);

                    /* Initializes the Listener */
                    myList = new TcpListener(ipAd, 8001);
                    /* Start Listeneting at the specified port */
                    myList.Start();
                    data.AppendText("The server is running at port 8001...");
                    //data.AppendText(Environment.NewLine);
                    data.AppendText("The local End point is  :" + myList.LocalEndpoint);
                    //data.AppendText(Environment.NewLine);
                    data.AppendText("...Waiting for a connection...");
                    //data.AppendText(Environment.NewLine);
                    s = await myList.AcceptSocketAsync();
                    data.AppendText("Connection accepted from " + s.RemoteEndPoint);
                    data.AppendText(Environment.NewLine);
                    
                    connection = true;
                }
                catch (SocketException connectexcept)
                {
                    data.AppendText("ERROR" + connectexcept.ToString());
                    //data.AppendText(Environment.NewLine);
                }
            }
            //while(true) readTcp();
        }

        private async void connectPipe()
        {
            data.AppendText("Waiting on Pipe Connection...");
            //data.AppendText(Environment.NewLine);

            // Connect to Pipe
            //kClient.Connect();
            kServer.WaitForConnection();
            sr = new StreamReader(kServer);
            sw = new StreamWriter(kServer) { AutoFlush = true };

            data.AppendText("The Pipe is Open");
            data.AppendText(Environment.NewLine);
        }

        private async void readTcp()
        {
            while (s == null) Thread.Sleep(1000);

            try
            {
                int k = s.Receive(b);
                string rev = "Received " + k + " bytes...";
                if (k > 0)
                {
                    data.AppendText(rev);
                    //data.AppendText(Environment.NewLine);
                }
                else
                {
                    data.AppendText("Error no bytes");
                    data.AppendText(Environment.NewLine);
                }
            }
            catch(SocketException readexcept)
            {
                data.AppendText("ERROR" + readexcept.ToString());
                //data.AppendText(Environment.NewLine);
            }
            readresult = Encoding.UTF8.GetString(b);

            data.AppendText(readresult);
            data.AppendText(Environment.NewLine);
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
                string ipadd = GetLocalIPAddress();
                data.AppendText(ipadd);
                IPAddress ipAd = IPAddress.Parse(ipadd);

                data.AppendText(Environment.NewLine);

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

                string res;
                res = sr.ReadLine();

                data.AppendText(res);
                data.AppendText(Environment.NewLine);

                byte[] b = new byte[buffer_len];
                //Thread.Sleep(10000);

                while (true)
                {
                    Thread.Sleep(700);
                    int k = s.Receive(b);
                    string rev = "Received " + k + " bytes...";
                    if (k > 0)
                    {
                        data.AppendText(rev);
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

        private async void Starttunnel()
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
                string ipadd = GetLocalIPAddress();
                data.AppendText(ipadd);
                IPAddress ipAd = IPAddress.Parse(ipadd);

                data.AppendText(Environment.NewLine);

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

                //string res;
                bool transmit = true;
                //res = sr.ReadLine();
                //string[] res_array = res.Split(',');
                //if (res_array[0] == "start") transmit = true;

                byte[] b = new byte[buffer_len];
                //Thread.Sleep(10000);

                while (true)
                {
                    Thread.Sleep(700);
                    int k = s.Receive(b);
                    string rev = "Received " + k + " bytes...";
                    if (k > 0)
                    {
                        data.AppendText(rev);
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
                    string[] time = result.Split(',');

                    //if (time >= res_array[1] && transmit == true) sw.WriteLine(result);


                    if (transmit == true) sw.WriteLine(result);


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

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local Ip Address not Found");

        }


    }
}