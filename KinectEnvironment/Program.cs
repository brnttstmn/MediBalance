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
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new KinectForm());

            var pipeClient = new NamedPipeClientStream(".", "kinect", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);
            pipeClient.Connect();
            var write = new StreamWriter(pipeClient) { AutoFlush = true };
            var read = new StreamReader(pipeClient);
            var message = "";

            while (message != "exit")
            {
                message = Console.ReadLine();
                write.WriteLine(message);
            }


            pipeClient.Close();
        }
    }
}
