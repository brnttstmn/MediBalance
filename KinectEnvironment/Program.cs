using System;
using System.Windows.Forms;
using Microsoft.Kinect;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace KinectEnvironment
{
    class Program
    {
        KinectSensor kinectSensor = KinectSensor.GetDefault();
        BodyFrameReader bodyframeReader = null;
        Body[] bodies = null;
        StringBuilder csv = new StringBuilder();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new KinectForm());
            if (args.Length < 2)
            {
                return -1;
            }
            else
            {
                IPAddress ip = IPAddress.Parse(args[0]);
                int port = Convert.ToInt32(args[1]);
                try { server(ip, port); }
                catch { return -1; }
            }
            return 0;
        }

        static int server(IPAddress ip, int port) {
            while (true) {
                try
                {
                    /* Initializes the Listener */
                    TcpListener myList = new TcpListener(ip, port);

                    /* Start Listeneting at the specified port */
                    myList.Start();

                    Console.WriteLine("The server is running at port "+port+" ...");
                    Console.WriteLine("The local End point is  :" +
                                      myList.LocalEndpoint);
                    Console.WriteLine("Waiting for a connection.....");

                    Socket s = myList.AcceptSocket();
                    Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);

                    byte[] b = new byte[100];
                    int k = s.Receive(b);
                    System.Diagnostics.Debug.WriteLine("Recieved...");


                    string result = Encoding.ASCII.GetString(b);
                    System.Diagnostics.Debug.WriteLine(result);

                    //for (int i = 0; i < k; i++)
                    //  Console.Write(Convert.ToChar(b[i]));

                    ASCIIEncoding asen = new ASCIIEncoding();
                    s.Send(asen.GetBytes("The string was recieved by the server."));
                    System.Diagnostics.Debug.WriteLine("\nSent Acknowledgement");

                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Error..... " + e.StackTrace);
                }
                Console.ReadLine();
        }
            return 0;
        }
    }
}
