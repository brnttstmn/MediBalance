using System;
using System.Windows.Forms;
using Microsoft.Kinect;
using System.Text;
using System.IO.Pipes;
using System.Net;
using System.Net.Sockets;
using System.Security.Principal;
using System.IO;
using System.Threading;

namespace KinectEnvironment
{
    class Program
    {
        static NamedPipeClientStream kClient = new NamedPipeClientStream(".", "kinect", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);
        KinectSensor kinectSensor = KinectSensor.GetDefault();
        BodyFrameReader bodyframeReader = null;
        Body[] bodies = null;
        StringBuilder csv = new StringBuilder();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Declare Variables
            var command = "";
            bool run = true;

            // Connect to Pipe
            kClient.Connect();

            // Run until told to exit
            while (run) {

                // Listen for command
                command = listen();
                Console.WriteLine(command);

                // Perform Command
                switch (command)
                {
                    case "start":
                        start();
                        break;
                    case "test":
                        test();
                        break;
                    case "stop":
                        run = false;
                        break;
                    default:
                        run = true;
                        break;
                }
            }

            // Close Pipe Connection -- Terminate program
            kClient.Close();
        }
        static string listen()
        {
            var read = new StreamReader(kClient);
            var message = "";

            while (true)
            {
                message = read.ReadLine();
                if (message != null)
                {
                    return message;
                }
                Thread.Sleep(500);
            }
        }

        static void start() { }

        static void test() {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new KinectForm());
        }
    }

}
