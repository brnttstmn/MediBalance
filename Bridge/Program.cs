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
        static NamedPipeServerStream kServer =  null;
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

        static ThreadStart pipestart = new ThreadStart(connectPipe);
        static Thread pipeconnection = null;

        static ThreadStart tcpstart = new ThreadStart(connectTcp);
        static Thread tcpconnection = null;

        static ThreadStart tcpreadstart = new ThreadStart(readnostream);
        static Thread tcpread = null;

        static ThreadStart tcpreadstreamstart = new ThreadStart(readNstream);
        static Thread tcpreadstream = new Thread(tcpreadstreamstart);

        static ThreadStart pipelistenstart = new ThreadStart(pipelistener);
        static Thread pipelisten = null;
        
        static void Main(string[] args)
        {
            //Console.WriteLine("In Main: Creating the Child threads");

            tcpconnection = new Thread(tcpstart);
            pipeconnection = new Thread(pipestart);
            pipelisten = new Thread(pipelistenstart);
            tcpread = new Thread(tcpreadstart);
            pipelisten = new Thread(pipelistenstart);


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
                if (!pipeconnect)
                {
                    if (!pipeconnection.IsAlive)
                    {
                        pipeconnection = new Thread(pipestart);
                        pipelisten = new Thread(pipelistenstart);
                        pipeconnection.Start();
                        pipelisten.Start();
                    }
                }
                threadmanage();
            }


            Console.WriteLine("Main: Finsihed");
            Console.ReadKey();
        }

        public static void threadmanage()
        {
            if (pipemsg == "Start" && pipeconnect == true)
            {
                if (!tcpreadstream.IsAlive)
                {
                    tcpreadstream = new Thread(tcpreadstreamstart);
                    try
                    {
                        tcpread.Abort();
                        tcpread.Join();
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
                    tcpread = new Thread(tcpreadstart);
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
                try
                {
                    sw.WriteLine(data);
                }
                catch (IOException)
                {
                    pipemsg = "Stop";
                    pipeconnect = false;
                    kServer.Close();
                }
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
            kServer = new NamedPipeServerStream("tunnel", PipeDirection.InOut);
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
            while (!pipeconnect) Thread.Sleep(10);
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
