using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.IO.Pipes;
namespace ConsoleApplication1
{
    class Program
    {
        static NamedPipeServerStream kServer = new NamedPipeServerStream("tunnel", PipeDirection.InOut);
        static StreamWriter sw = null;
        static StreamReader sr = null;
        static IPAddress ipAd = null;
        /* Creates the TCP Listener and Socket */
        static TcpListener myList = null;
        static Socket s = null;
        //Create receive buffer for TCP connection
        static int buffer_len = 27;
        static byte[] b = new byte[buffer_len];
        static string readresult = null;
        static bool tcpconnect = false;
        static bool pipeconnect = false;
        static string pipemsg = "Stop";

        static void Main(string[] args)
        {
            Console.WriteLine("In Main: Creating the Child threads");

            ThreadStart pipestart = new ThreadStart(connectPipe);
            Thread pipeconnection = new Thread(pipestart);

            ThreadStart tcpstart = new ThreadStart(connectTcp);
            Thread tcpconnection = new Thread(tcpstart);

            ThreadStart tcpreadstart = new ThreadStart(readnostream);
            Thread tcpread = new Thread(tcpreadstart);

            ThreadStart tcpreadstreamstart = new ThreadStart(readNstream);
            Thread tcpreadstream = new Thread(tcpreadstreamstart);

            ThreadStart pipelistenstart = new ThreadStart(pipelistener);
            Thread pipelisten = new Thread(pipelistenstart);


            //Start connection threads(they act weird)
            tcpconnection.Start();
            pipeconnection.Start();

            //Console.WriteLine(isalive); //DEBug

            //WAIT for either or both the tcp or piipe connection
            while (!tcpconnect || !pipeconnect) Thread.Sleep(1);
            //while (!tcpconnect) Thread.Sleep(1);
            //while (!pipeconnect) Thread.Sleep(1);
            
            pipelisten.Start();
            tcpread.Start();

            while (true)
            {
                if (pipemsg == "Start")
                {
                    if (!tcpreadstream.IsAlive)
                    {
                        try
                        {
                            tcpread.Abort();
                            //tcpread.Join();
                            tcpreadstream.Start();
                            pipemsg = "Running";
                        }
                        catch (Exception)
                        {
                            tcpreadstream.Start();
                            pipemsg = "Running";
                        }
                    }
                }
                if (pipemsg == "Stop")
                {
                    if (!tcpread.IsAlive)
                    {

                        try
                        {
                            tcpreadstream.Abort();
                            tcpreadstream.Join();
                            tcpread.Start();
                            pipemsg = "Stopped";
                        }
                        catch (Exception)
                        {
                            tcpread.Start();
                            pipemsg = "Stopped";
                        }
                    }
                }
            }


            Console.WriteLine("Main: Finsihed");
            Console.ReadKey();
        }

        public static async void connectTcp()
        {
            tcpconnect = false;
            Console.WriteLine("CONNECTING TCP");
            while (!tcpconnect)
            {
                try
                {
                    string ipadd = GetLocalIPAddress();
                    //data.AppendText(ipadd);
                    ipAd = IPAddress.Parse(ipadd);

                    /* Initializes the Listener */
                    myList = new TcpListener(ipAd, 8001);
                    /* Start Listeneting at the specified port */
                    myList.Start();
                    Console.WriteLine("The server is running at port 8001...");
                    //data.AppendText(Environment.NewLine);
                    Console.WriteLine("The local End point is  :" + myList.LocalEndpoint);
                    //data.AppendText(Environment.NewLine);
                    Console.WriteLine("...Waiting for a connection...");
                    //data.AppendText(Environment.NewLine);
                    s = await myList.AcceptSocketAsync();
                    Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);
                    tcpconnect = true;
                }
                catch (SocketException connectexcept)
                {
                    Console.WriteLine("ERROR" + connectexcept.ToString());
                    //data.AppendText(Environment.NewLine);
                }
            }
            //while(true) readTcp();
        }

        static public void readnostream()
        {
            Console.WriteLine("Readnostream started");
            while (true)
            {
                readTcp();
            }
        }

        static public void readNstream()
        {
            string data;
            Console.WriteLine("ReadNstream started");
            while (true)
            {
                data = readTcp();
                sw.WriteLine(data);
            }
        }


        static public string readTcp()
        {
            while (s == null) Thread.Sleep(1000);

            try
            {
                int k = s.Receive(b);
                string rev = "Received " + k + " bytes...";
                if (k > 0)
                {
                    Console.WriteLine(rev);
                    //data.AppendText(Environment.NewLine);
                }
                else
                {
                    Console.WriteLine("Error no bytes");
                }
            }
            catch (SocketException readexcept)
            {
                Console.WriteLine("ERROR" + readexcept.ToString());
            }
            readresult = Encoding.UTF8.GetString(b);

            Console.WriteLine(readresult);
            return readresult;
        }


        static public void connectPipe()
        {
            pipeconnect = false;
            Console.WriteLine("Waiting on Pipe Connection...");
            //data.AppendText(Environment.NewLine);

            // Connect to Pipe
            //kClient.Connect();
            kServer.WaitForConnection();
            sr = new StreamReader(kServer);
            sw = new StreamWriter(kServer) { AutoFlush = true };
            pipeconnect = true;
            Console.WriteLine("The Pipe is Open");
        }

        static public void pipelistener()
        {
            //pipemsg = null;
            string res;
            res = sr.ReadLine();
            pipemsg = res;
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
