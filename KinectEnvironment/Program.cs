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
        static NamedPipeServerStream kServer;
        static StreamWriter StreamWrite = null;
        static StreamReader StreamRead = null;
        static bool isRead = false;
        static bool connectionBroken = false;
        // Create Objects
        static KinectSensor kinectSensor = KinectSensor.GetDefault();
        static BodyFrameReader bodyframeReader = null;


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            run();
        }

        static void run()
        {
            // Initialize Variables
            var run = true;

            while (run)
            {
                try
                {
                    // Connect to Pipe
                    Console.WriteLine("Connecting....");
                    connectPipe();
                    listen();
                    startRead();
                    while (true)
                    {
                        if (connectionBroken) { connectionBroken = false; throw new IOException(); }
                        //Thread.Sleep(1000);
                        //StreamWrite.WriteLine();
                    }
                }
                catch (IOException) { Console.WriteLine("Connection Terminated"); }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); run = false; }
                finally
                {
                    kinectSensor.Close();
                    kServer.Dispose();
                    isRead = false;
                }
            }
        }

        private static void connectPipe()
        {
            // Start and Connect to Pipe
            kServer = new NamedPipeServerStream("kinect", PipeDirection.InOut);
            Console.WriteLine("Waiting for Connection...");
            kServer.WaitForConnection();
            StreamRead = new StreamReader(kServer);
            StreamWrite = new StreamWriter(kServer) { AutoFlush = true };
            Console.WriteLine("Pipe Connected.");
        }

        static string listen()
        {
            var message = "";

            while (true)
            {
                message = StreamRead.ReadLine();
                if (message != null)
                {
                    return message;
                }
            }
        }

        static void startRead()
        {
            kinectSensor.Open();
            // Track Changes
            bodyframeReader = kinectSensor.BodyFrameSource.OpenReader();
            if (bodyframeReader != null)
            {
                bodyframeReader.FrameArrived += BodyframeReader_FrameArrived;
            }
            isRead = true;
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
                            string timestamp = DateTime.Now.ToString(timeFormat);
                            try
                            {
                                foreach (KeyValuePair<Joint, string> joint in jointName)
                                {
                                    var newLine = string.Format("{0},{1},{2},{3},{4};", timestamp, joint.Value,
                                    joint.Key.Position.X.ToString(), joint.Key.Position.Y.ToString(),
                                    joint.Key.Position.Z.ToString());
                                    StreamWrite.WriteLine(newLine);
                                    Console.WriteLine("Written");
                                    Console.WriteLine(newLine);
                                }
                            }
                            catch (IOException) { connectionBroken = true; }
                        }
                    }
                }
            }
        }
    }
}