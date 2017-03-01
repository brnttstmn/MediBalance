using System;
using Microsoft.Kinect;
using System.Text;
using System.IO.Pipes;
using System.Security.Principal;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Drawing;

namespace KinectEnvironment
{
    class Program
    {
        static string timeFormat = "HH:mm:ss:fff";
        static NamedPipeServerStream kServer = new NamedPipeServerStream("kinect", PipeDirection.InOut);
        static StreamWriter sw = null;
        static StreamReader sr = null;
        static int count = 0;


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
            //kClient.Connect();
            kServer.WaitForConnection();
            sr = new StreamReader(kServer);
            sw = new StreamWriter(kServer) { AutoFlush = true };

            // Run until told to exit
            while (run)
            {

                // Listen for command
                Console.WriteLine("Listening");
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
                //temporary. needs debugging
                run = false;
            }

            // Close Pipe Connection -- Terminate program
            //kClient.Close();
            kServer.Close();

            Console.WriteLine("Press Enter to Continue...");
            Console.ReadLine();
        }

        static string listen()
        {
            var message = "";

            while (true)
            {
                message = sr.ReadLine();
                if (message != null)
                {
                    return message;
                }
                Thread.Sleep(500);
            }
        }

        static void start()
        {
            // Create Objects
            KinectSensor kinectSensor = KinectSensor.GetDefault();
            BodyFrameReader bodyframeReader = null;


            // Start Kinect
            kinectSensor.Open();

            // Track Changes
            bodyframeReader = kinectSensor.BodyFrameSource.OpenReader();
            if (bodyframeReader != null)
            {
                bodyframeReader.FrameArrived += BodyframeReader_FrameArrived;
            }

            int i = 0;
            // Wait for 15s
            while (true)
            {
                Console.WriteLine(i++);
                Thread.Sleep(1000);
            }

            // Close Kinect Connection
            kinectSensor.Close();
        }

        static void test()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new KinectForm());
        }

        private static void BodyframeReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            Body[] person = null;
            bool dataReceived = false;

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (person == null)
                    {
                        person = new Body[bodyFrame.BodyCount];
                    }

                    bodyFrame.GetAndRefreshBodyData(person);
                    dataReceived = true;
                }

                if (dataReceived)
                {
                    foreach (Body part in person)
                    {
                        if (part.IsTracked)
                        {
                            IReadOnlyDictionary<JointType, Joint> joints = part.Joints;

                            Joint spinebase = joints[JointType.SpineBase];
                            Joint midspine = joints[JointType.SpineMid];
                            Joint neck = joints[JointType.Neck];
                            Joint head = joints[JointType.Head];
                            Joint shoulderleft = joints[JointType.ShoulderLeft];
                            Joint elbowleft = joints[JointType.ElbowLeft];
                            Joint wristleft = joints[JointType.WristLeft];
                            Joint handleft = joints[JointType.HandLeft];
                            Joint shoulderright = joints[JointType.ShoulderRight];
                            Joint elbowright = joints[JointType.ElbowRight];
                            Joint wristright = joints[JointType.WristRight];
                            Joint handright = joints[JointType.HandRight];
                            Joint hipleft = joints[JointType.HipLeft];
                            Joint kneeleft = joints[JointType.KneeLeft];
                            Joint footleft = joints[JointType.FootLeft];
                            Joint hipright = joints[JointType.HipRight];
                            Joint kneeright = joints[JointType.KneeRight];
                            Joint footright = joints[JointType.FootRight];

                            var jointName = new Dictionary<Joint, string>()
                            {
                                {spinebase, "spinebase"}, { midspine, "midspine"}, {neck, "neck"}, {head,"head"}, {shoulderleft,"shoulderleft"},
                                {elbowleft,"elbowleft"}, {wristleft, "wristleft"}, {handleft,"handleft"}, {shoulderright,"shoulderright"},
                                {elbowright,"elbowright"}, {wristright,"wristright"}, {handright,"handright"}, {hipleft,"hipleft"},
                                {kneeleft,"kneeleft"}, {footleft,"footleft"}, {hipright,"hipright"}, {kneeright,"kneeright"}, {footright,"footright"}
                            };

                            foreach (KeyValuePair<Joint, string> joint in jointName)
                            {
                                var newLine = string.Format("{0},{1},{2},{3},{4};", DateTime.Now.ToString(timeFormat), joint.Value,
                                joint.Key.Position.X.ToString(), joint.Key.Position.Y.ToString(),
                                joint.Key.Position.Z.ToString());
                                sw.WriteLine(newLine);
                                Console.WriteLine(newLine);
                            }
                        }
                    }
                }
            }
        }
    }
}