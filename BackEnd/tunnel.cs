using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BackEnd
{
    class tunnel
    {
        private string line;
        private List<string> data;
        
        public async void connect()
        {
            NamedPipeServerStream kServer = new NamedPipeServerStream("tunnel", PipeDirection.InOut);
            StreamWriter sw = null;
            StreamReader sr = null;

            data.Add("Waiting on Pipe Connection...");
            //data.AppendText(Environment.NewLine);


            // Connect to Pipe
            //kClient.Connect();
            kServer.WaitForConnection();
            sr = new StreamReader(kServer);
            sw = new StreamWriter(kServer) { AutoFlush = true };

            data.Add("The Pipe is Open");
            data.Add(Environment.NewLine);



            try
            {
                string ipadd = GetLocalIPAddress();
                data.Add(ipadd);
                IPAddress ipAd = IPAddress.Parse(ipadd);

                data.Add(Environment.NewLine);

                // use local m/c IP address, and 
                // use the same in the client

                /* Initializes the Listener */
                TcpListener myList = new TcpListener(ipAd, 8001);

                /* Start Listeneting at the specified port */
                myList.Start();

                data.Add("The server is running at port 8001...");
                //data.AppendText(Environment.NewLine);


                data.Add("The local End point is  :" + myList.LocalEndpoint);
                //data.AppendText(Environment.NewLine);

                data.Add("...Waiting for a connection...");
                //data.AppendText(Environment.NewLine);


                Socket s = await myList.AcceptSocketAsync();
                data.Add("Connection accepted from " + s.RemoteEndPoint);
                data.Add(Environment.NewLine);

                byte[] b = new byte[100];
                Thread.Sleep(10000);

                while (true)
                {
                    Thread.Sleep(3000);
                    int k = s.Receive(b);
                    if (k > 0)
                    {
                        data.Add("Received...");
                        //data.AppendText(Environment.NewLine);
                    }
                    else
                    {
                        data.Add("Error no bytes");
                        data.Add(Environment.NewLine);
                    }

                    string result = Encoding.UTF8.GetString(b);

                    data.Add(result);
                    data.Add(Environment.NewLine);

                    sw.WriteLine(result);

                    //Encoding utf = Encoding.UTF8;
                    //s.Send(utf.GetBytes(msg));
                    data.Add("Data Passed to Backend");
                    data.Add(Environment.NewLine);
                }
                /* clean up */
                s.Close();
                myList.Stop();



            }
            catch (SocketException y)
            {
                data.Add("Error..." + y.StackTrace);
                data.Add(Environment.NewLine);
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
