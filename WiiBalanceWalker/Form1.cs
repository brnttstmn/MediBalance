using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Kinect;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace WiiBalanceWalker
{
    public partial class Form1 : Form
    {
        KinectSensor kinectSensor = KinectSensor.GetDefault();
        BodyFrameReader bodyframeReader = null;
        Body[] bodies = null;
        StringBuilder csv = new StringBuilder();

        private void Form1_Load(object sender, EventArgs e)
        { }
            public Form1()
        {
            InitializeComponent(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
             if (button2.Text == "Stream")
            {
                this.button2.Text = "Stop";
                var newLine = string.Format("Joint,X,Y,Z");
                csv.AppendLine(newLine);
                kinectSensor.Open();

                bodyframeReader = kinectSensor.BodyFrameSource.OpenReader();

                if (bodyframeReader != null)
                {
                    bodyframeReader.FrameArrived += BodyframeReader_FrameArrived;
                }
            }
            else
            {
                if(kinectSensor != null && kinectSensor.IsOpen)
                {
                    kinectSensor.Close();
                    this.button2.Text = "Stream";
                    File.WriteAllText("C:\\Users\\Shawn\\Documents\\CSV", csv.ToString());
                }
            }
        }
       
        private void BodyframeReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            bool dataReceived = false;

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if(bodyFrame != null)
                {
                    if (bodies == null)
                    {
                        bodies = new Body[bodyFrame.BodyCount];
                    }

                    bodyFrame.GetAndRefreshBodyData(bodies);
                    dataReceived = true;
                }

                if (dataReceived)
                {
                    foreach(Body body in bodies)
                    {
                        if (body.IsTracked)
                        {
                            IReadOnlyDictionary<JointType, Joint> joints = body.Joints;
                            Dictionary<JointType, Point> jointpoints = new Dictionary<JointType, Point>();

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



                            var newLine = string.Format("SpineBase,{0},{1},{2}",
                                spinebase.Position.X.ToString(), spinebase.Position.Y.ToString(), 
                                spinebase.Position.Z.ToString());
                            csv.AppendLine(newLine);

                            newLine = string.Format("SpineMid,{0},{1},{2}",
                                midspine.Position.X.ToString(),midspine.Position.Y.ToString(),
                                midspine.Position.Z.ToString());
                            csv.AppendLine(newLine);

                            txtBaseSpineX.Text = spinebase.Position.X.ToString("#.##");
                            txtBaseSpineY.Text = spinebase.Position.Y.ToString("#.##");
                            txtBaseSpineZ.Text = spinebase.Position.Z.ToString("#.##");

                            txtMidSpineX.Text = midspine.Position.X.ToString("#.##");
                            txtMidSpineY.Text = midspine.Position.Y.ToString("#.##");
                            txtMidSpineZ.Text = midspine.Position.Z.ToString("#.##");

                            txtLeftShoulderX.Text = shoulderleft.Position.X.ToString("#.##");
                            txtLeftShoulderY.Text = shoulderleft.Position.Y.ToString("#.##");
                            txtLeftShoulderZ.Text = shoulderleft.Position.Z.ToString("#.##");

                            
                            txtLeftElbowX.Text = elbowleft.Position.X.ToString("#.##");
                            txtLeftElbowY.Text = elbowleft.Position.Y.ToString("#.##");
                            txtLeftElbowZ.Text = elbowleft.Position.Z.ToString("#.##");

                            
                            txtLeftHandX.Text = handleft.Position.X.ToString("#.##");
                            txtLeftHandY.Text = handleft.Position.Y.ToString("#.##");
                            txtLeftHandZ.Text = handleft.Position.Z.ToString("#.##");

                        }
                    }
                }
            }
        }

  
    }
}
