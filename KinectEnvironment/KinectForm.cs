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

namespace KinectEnvironment
{
    public partial class KinectForm : Form
    {
        KinectSensor kinectSensor = KinectSensor.GetDefault();
        BodyFrameReader bodyframeReader = null;
        Body[] bodies = null;
        StringBuilder csv = new StringBuilder();

        private void Form1_Load(object sender, EventArgs e)
        { }
            public KinectForm()
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
                if (kinectSensor != null && kinectSensor.IsOpen)
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
                if (bodyFrame != null)
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
                    foreach (Body body in bodies)
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
                                midspine.Position.X.ToString(), midspine.Position.Y.ToString(),
                                midspine.Position.Z.ToString());
                            csv.AppendLine(newLine);

                            newLine = string.Format("Neck,{0},{1},{2}",
                                neck.Position.X.ToString(), neck.Position.Y.ToString(),
                                neck.Position.Z.ToString());
                            csv.AppendLine(newLine);

                            newLine = string.Format("Head,{0},{1},{2}",
                                head.Position.X.ToString(), head.Position.Y.ToString(),
                                head.Position.Z.ToString());
                            csv.AppendLine(newLine);

                            newLine = string.Format("ShoulderLeft,{0},{1},{2}",
                                shoulderleft.Position.X.ToString(), shoulderleft.Position.Y.ToString(),
                                shoulderleft.Position.Z.ToString());
                            csv.AppendLine(newLine);

                            newLine = string.Format("ElbowLeft,{0},{1},{2}",
                                elbowleft.Position.X.ToString(), elbowleft.Position.Y.ToString(),
                                elbowleft.Position.Z.ToString());
                            csv.AppendLine(newLine);

                            newLine = string.Format("WristLeft,{0},{1},{2}",
                                wristleft.Position.X.ToString(), wristleft.Position.Y.ToString(),
                                wristleft.Position.Z.ToString());
                            csv.AppendLine(newLine);

                            newLine = string.Format("HandLeft,{0},{1},{2}",
                                handleft.Position.X.ToString(), handleft.Position.Y.ToString(),
                                handleft.Position.Z.ToString());
                            csv.AppendLine(newLine);

                            newLine = string.Format("ShoulderRight,{0},{1},{2}",
                                shoulderright.Position.X.ToString(), shoulderright.Position.Y.ToString(),
                                shoulderright.Position.Z.ToString());
                            csv.AppendLine(newLine);

                            newLine = string.Format("ElbowRight,{0},{1},{2}",
                                elbowright.Position.X.ToString(), elbowright.Position.Y.ToString(),
                                elbowright.Position.Z.ToString());
                            csv.AppendLine(newLine);

                            newLine = string.Format("WristRight,{0},{1},{2}",
                                wristright.Position.X.ToString(), wristright.Position.Y.ToString(),
                                wristright.Position.Z.ToString());
                            csv.AppendLine(newLine);

                            newLine = string.Format("HandRight,{0},{1},{2}",
                                handright.Position.X.ToString(), handright.Position.Y.ToString(),
                                handright.Position.Z.ToString());
                            csv.AppendLine(newLine);

                            newLine = string.Format("HipLeft,{0},{1},{2}",
                                hipleft.Position.X.ToString(), hipleft.Position.Y.ToString(),
                                hipleft.Position.Z.ToString());
                            csv.AppendLine(newLine);

                            newLine = string.Format("KneeLeft,{0},{1},{2}",
                                kneeleft.Position.X.ToString(), kneeleft.Position.Y.ToString(),
                                kneeleft.Position.Z.ToString());
                            csv.AppendLine(newLine);

                            newLine = string.Format("FootLeft,{0},{1},{2}",
                                footleft.Position.X.ToString(), footleft.Position.Y.ToString(),
                                footleft.Position.Z.ToString());
                            csv.AppendLine(newLine);

                            newLine = string.Format("HipRight,{0},{1},{2}",
                                hipright.Position.X.ToString(), hipright.Position.Y.ToString(),
                                hipright.Position.Z.ToString());
                            csv.AppendLine(newLine);

                            newLine = string.Format("KneeRight,{0},{1},{2}",
                                kneeright.Position.X.ToString(), kneeright.Position.Y.ToString(),
                                kneeright.Position.Z.ToString());
                            csv.AppendLine(newLine);

                            newLine = string.Format("FootRight,{0},{1},{2}",
                                footright.Position.X.ToString(), footright.Position.Y.ToString(),
                                footright.Position.Z.ToString());
                            csv.AppendLine(newLine);

                            txtBaseSpineX.Text = spinebase.Position.X.ToString("#.##");
                            txtBaseSpineY.Text = spinebase.Position.Y.ToString("#.##");
                            txtBaseSpineZ.Text = spinebase.Position.Z.ToString("#.##");

                            txtMidSpineX.Text = midspine.Position.X.ToString("#.##");
                            txtMidSpineY.Text = midspine.Position.Y.ToString("#.##");
                            txtMidSpineZ.Text = midspine.Position.Z.ToString("#.##");

                            txtNeckX.Text = neck.Position.X.ToString("#.##");
                            txtNeckY.Text = neck.Position.Y.ToString("#.##");
                            txtNeckZ.Text = neck.Position.Z.ToString("#.##");

                            txtHeadX.Text = head.Position.X.ToString("#.##");
                            txtHeadY.Text = head.Position.Y.ToString("#.##");
                            txtHeadZ.Text = head.Position.Z.ToString("#.##");

                            txtLeftShoulderX.Text = shoulderleft.Position.X.ToString("#.##");
                            txtLeftShoulderY.Text = shoulderleft.Position.Y.ToString("#.##");
                            txtLeftShoulderZ.Text = shoulderleft.Position.Z.ToString("#.##");


                            txtLeftElbowX.Text = elbowleft.Position.X.ToString("#.##");
                            txtLeftElbowY.Text = elbowleft.Position.Y.ToString("#.##");
                            txtLeftElbowZ.Text = elbowleft.Position.Z.ToString("#.##");

                            txtLeftWristX.Text = wristleft.Position.X.ToString("#.##");
                            txtLeftWristY.Text = wristleft.Position.Y.ToString("#.##");
                            txtLeftWristZ.Text = wristleft.Position.Z.ToString("#.##");

                            txtLeftHandX.Text = handleft.Position.X.ToString("#.##");
                            txtLeftHandY.Text = handleft.Position.Y.ToString("#.##");
                            txtLeftHandZ.Text = handleft.Position.Z.ToString("#.##");

                            txtRightShoulderX.Text = shoulderright.Position.X.ToString("#.##");
                            txtRightShoulderY.Text = shoulderright.Position.Y.ToString("#.##");
                            txtRightShoulderZ.Text = shoulderright.Position.Z.ToString("#.##");


                            txtRightElbowX.Text = elbowright.Position.X.ToString("#.##");
                            txtRightElbowY.Text = elbowright.Position.Y.ToString("#.##");
                            txtRightElbowZ.Text = elbowright.Position.Z.ToString("#.##");

                            txtRightWristX.Text = wristright.Position.X.ToString("#.##");
                            txtRightWristY.Text = wristright.Position.Y.ToString("#.##");
                            txtRightWristZ.Text = wristright.Position.Z.ToString("#.##");

                            txtRightHandX.Text = handright.Position.X.ToString("#.##");
                            txtRightHandY.Text = handright.Position.Y.ToString("#.##");
                            txtRightHandZ.Text = handright.Position.Z.ToString("#.##");

                            txtLeftHipX.Text = hipleft.Position.X.ToString("#.##");
                            txtLeftHipY.Text = hipleft.Position.Y.ToString("#.##");
                            txtLeftHipZ.Text = hipleft.Position.Z.ToString("#.##");

                            txtLeftKneeX.Text = kneeleft.Position.X.ToString("#.##");
                            txtLeftKneeY.Text = kneeleft.Position.Y.ToString("#.##");
                            txtLeftKneeZ.Text = kneeleft.Position.Z.ToString("#.##");

                            txtLeftFootX.Text = footleft.Position.X.ToString("#.##");
                            txtLeftFootY.Text = footleft.Position.Y.ToString("#.##");
                            txtLeftFootZ.Text = footleft.Position.Z.ToString("#.##");

                            txtRightHipX.Text = hipright.Position.X.ToString("#.##");
                            txtRightHipY.Text = hipright.Position.Y.ToString("#.##");
                            txtRightHipZ.Text = hipright.Position.Z.ToString("#.##");

                            txtRightKneeX.Text = kneeright.Position.X.ToString("#.##");
                            txtRightKneeY.Text = kneeright.Position.Y.ToString("#.##");
                            txtRightKneeZ.Text = kneeright.Position.Z.ToString("#.##");

                            txtRightFootX.Text = footright.Position.X.ToString("#.##");
                            txtRightFootY.Text = footright.Position.Y.ToString("#.##");
                            txtRightFootZ.Text = footright.Position.Z.ToString("#.##");
                        }
                    }
                }
            }
        }

    }
}
