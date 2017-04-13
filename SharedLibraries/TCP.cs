using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SharedLibraries
{
    public class TCP:Comm
    {
        // Objects
        protected IPAddress ip;
        protected TcpListener listener = null;
        protected Socket socket = null;
        protected const int buffer_len = 27;
        protected byte[] b = new byte[buffer_len];
        protected string readresult = null;


        public TCP()
        {
            ip = GetLocalIPAddress();
            type = typeof(TCP);
            streamActive = false;
        }

        public async void start()
        {
            Console.WriteLine("CONNECTING TCP");
            while (!streamActive)
            {
                try
                {
                    /* Initializes the Listener */
                    listener = new TcpListener(ip, 8001);

                    /* Start Listeneting at the specified port */
                    listener.Start();
                    Console.WriteLine("The server is running at port 8001...");
                    Console.WriteLine("The local End point is  :" + listener.LocalEndpoint);
                    Console.WriteLine("...Waiting for a connection...");
                    socket = await listener.AcceptSocketAsync();
                    Console.WriteLine("Connection accepted from " + socket.RemoteEndPoint);
                    streamActive = true;
                }
                catch (SocketException connectexcept)
                {
                    Console.WriteLine("ERROR" + connectexcept.ToString());
                }
            }
        }

        public void stop()
        {
            listener.Stop();
            socket.Close();
        }

        private void readNstream()
        {
            Console.WriteLine("ReadNstream started");
            while (true)
            {
                var data = readStream();
            }
        }

        private void readnostream()
        {
            Console.WriteLine("Readnostream started");
            while (true)
            {
                readStream();
            }
        }

        public string readStream()
        {
            while (socket == null) Thread.Sleep(1000);

            try
            {
                int k = socket.Receive(b);
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
