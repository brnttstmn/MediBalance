using System;
using Microsoft.Kinect;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using SharedLibraries;

namespace KinectEnvironment
{
    class Program
    {
        // Initialize Pipe
        static  Pipe kserver = new Pipe("kinect", false);
        static bool connectionBroken = false;

        // Create Kinect Objects
        static KinectSensor kinectSensor = KinectSensor.GetDefault();
        static BodyFrameReader bodyframeReader = null;


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Start Kinect Pipe Server
            kserver.start();

            // Start Kinect Read Event
            startRead();

            // Run untill Pipe connection is broken
            while (!connectionBroken)
            {
                Thread.Sleep(5);
            }

            // Disconnect Pipe and Kinect
            Console.WriteLine("Connection Terminated");
            kinectSensor.Close();
            kserver.stop();
        }

        /// <summary>
        /// Start Kinect capture event
        /// </summary>
        static void startRead()
        {
            // Start Kinect sensor
            kinectSensor.Open();

            // Track Changes
            bodyframeReader = kinectSensor.BodyFrameSource.OpenReader();
            if (bodyframeReader != null)
            {
                bodyframeReader.FrameArrived += BodyframeReader_FrameArrived;
            }
        }

        /// <summary>
        /// Kinect capture event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                            string timestamp = DateTime.Now.ToString("HH:mm:ss:fff");
                            try
                            {
                                foreach (KeyValuePair<Joint, string> joint in jointName)
                                {
                                    var newLine = string.Format("{0},{1},{2},{3},{4};", timestamp, joint.Value,
                                    joint.Key.Position.X.ToString(), joint.Key.Position.Y.ToString(),
                                    joint.Key.Position.Z.ToString());
                                    kserver.send(newLine);
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