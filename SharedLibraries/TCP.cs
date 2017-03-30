using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharedLibraries
{
    public class TCP
    {
        // Objects
        static IPAddress ip;
        static TcpListener myList = null;
        static Socket s = null;
        static int buffer_len = 27;
        static byte[] b = new byte[buffer_len];
        static string readresult = null;
        static bool tcpconnect = false;


        public TCP()
        {
            ip = GetLocalIPAddress();
        }

        public static async void connectTcp()
        {
            tcpconnect = false;
            Console.WriteLine("CONNECTING TCP");
            while (!tcpconnect)
            {
                try
                {
                    /* Initializes the Listener */
                    myList = new TcpListener(ip, 8001);

                    /* Start Listeneting at the specified port */
                    myList.Start();
                    Console.WriteLine("The server is running at port 8001...");
                    Console.WriteLine("The local End point is  :" + myList.LocalEndpoint);
                    Console.WriteLine("...Waiting for a connection...");
                    s = await myList.AcceptSocketAsync();
                    Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);
                    tcpconnect = true;
                }
                catch (SocketException connectexcept)
                {
                    Console.WriteLine("ERROR" + connectexcept.ToString());
                }
            }
            //while(true) readTcp();
        }

        private void readNstream()
        {
            Console.WriteLine("ReadNstream started");
            while (true)
            {
                var data = readTcp();
            }
        }

        private void readnostream()
        {
            Console.WriteLine("Readnostream started");
            while (true)
            {
                readTcp();
            }
        }

        private string readTcp()
        {
            while (s == null) Thread.Sleep(1000);

            try
            {
                int k = s.Receive(b);
                string rev = "Received " + k + " bytes...";
                if (k > 0)
                {
                    Console.WriteLine(rev);
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


        private static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new Exception("Local Ip Address not Found");
        }
    }
}
