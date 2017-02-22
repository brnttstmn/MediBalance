using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Band;
using System.Threading.Tasks;
using System.Collections;

namespace MediBalance
{
    class MSBand2
    {
        string timeFormat = "HH:mm:ss:fff";
        /*
         * Class Variables
         */
        private IBandClient bandClient=null;

        /*
        * TSK Everything(int time, TextBlock OutputText):
        *  This method takes in an interger time and a textblock. The method will run for the given time
        *  and output the results live in the textbox. 
        *  
        *  **Note: Currently only running one sensor at a time (It can run all but not sure how we should output it...)
        * 
        */
        public async Task<int> everything(int RunTime,List<string> samples, BitArray control, Dictionary<string, int> map, TextBlock OutputText)
        {
            Tcp_Client clit = new Tcp_Client();
            clit.create_socket();
            clit.connect();

            OutputText.Visibility = Visibility.Visible;
            try
            {
                // Get the list of Microsoft Bands paired to the phone.
                IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
                if (pairedBands.Length < 1)
                {
                    return -1;
                }
                // Connect to Microsoft Band.
                using (bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]))
                {
                    if (control[map["hr"]])
                    {
                        bool heartRateConsentGranted;
                        // Check whether the user has granted access to the HeartRate sensor.
                        if (bandClient.SensorManager.HeartRate.GetCurrentUserConsent() == UserConsent.Granted)
                        { heartRateConsentGranted = true; }
                        else { heartRateConsentGranted = await bandClient.SensorManager.HeartRate.RequestUserConsentAsync(); }
                        if (!heartRateConsentGranted) { return -1; }
                    }
                    if (control[map["gsr"]])
                    {
                        bool gsrConsentGranted;
                        // Check whether the user has granted access to the HeartRate sensor.
                        if (bandClient.SensorManager.Gsr.GetCurrentUserConsent() == UserConsent.Granted)
                        { gsrConsentGranted = true; }
                        else { gsrConsentGranted = await bandClient.SensorManager.Gsr.RequestUserConsentAsync(); }
                        if (!gsrConsentGranted) { return -1; }
                    }
                    if (control[map["ls"]])
                    {
                        bool lsConsentGranted;
                        // Check whether the user has granted access to the HeartRate sensor.
                        if (bandClient.SensorManager.AmbientLight.GetCurrentUserConsent() == UserConsent.Granted)
                        { lsConsentGranted = true; }
                        else { lsConsentGranted = await bandClient.SensorManager.AmbientLight.RequestUserConsentAsync(); }
                        if (!lsConsentGranted) { return -1; }
                    }

                    if (control[map["hr"]]) {
                        // Subscribe to HeartRate data.
                        bandClient.SensorManager.HeartRate.ReadingChanged += async (s, args) =>
                        {
                            await Windows.ApplicationModel.Core.CoreApplication.MainView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                            {
                                OutputText.Text += string.Format("\n{1},Heartrate,{0};", DateTime.Now.ToString(timeFormat), args.SensorReading.HeartRate.ToString());
                                samples.Add(string.Format("\n{1},Heartrate,{0};", DateTime.Now.ToString(timeFormat), args.SensorReading.HeartRate.ToString()));
                                await clit.send(string.Format("\n{1},Heartrate,{0};", DateTime.Now.ToString(timeFormat), args.SensorReading.HeartRate.ToString()));
                            });
                        };
                        await bandClient.SensorManager.HeartRate.StartReadingsAsync();
                    }
                    if (control[map["gsr"]])
                    {
                        // Subscribe to GSR data.
                        bandClient.SensorManager.Gsr.ReadingChanged += async (s, args) =>
                        {
                            await Windows.ApplicationModel.Core.CoreApplication.MainView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                            {
                                OutputText.Text += string.Format("\ngsr,{1},{0};", args.SensorReading.Resistance, DateTime.Now.ToString(timeFormat));
                                samples.Add(string.Format("\ngsr,{1},{0};", args.SensorReading.Resistance, DateTime.Now.ToString(timeFormat)));
                            });
                        };
                        await bandClient.SensorManager.Gsr.StartReadingsAsync();
                    }
                    if (control[map["ls"]])
                    {
                        // Subscribe to light sensor data.
                        bandClient.SensorManager.AmbientLight.ReadingChanged += async (s, args) =>
                        {
                            await Windows.ApplicationModel.Core.CoreApplication.MainView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                            {
                                OutputText.Text += string.Format("\nlight,{1},{0};", args.SensorReading.Brightness, DateTime.Now.ToString(timeFormat));
                                samples.Add(string.Format("\nlight,{1},{0};", args.SensorReading.Brightness, DateTime.Now.ToString(timeFormat)));
                            });
                        };
                        await bandClient.SensorManager.AmbientLight.StartReadingsAsync();
                    }


                    // Run time to collect samples
                    await Task.Delay(TimeSpan.FromSeconds(RunTime));


                    // Shut off Sensors
                    await bandClient.SensorManager.HeartRate.StopReadingsAsync();
                    await bandClient.SensorManager.Gsr.StopReadingsAsync();
                    await bandClient.SensorManager.AmbientLight.StopReadingsAsync();
                    return 0;
                    }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        /*
         * Connect Band without reading Sensors
         * 
         * **Notes: This portion appears to work; however, when seperated methods are called,
         *      (startRead errors) errors out saying object has been removed--NEEDS WORK 
         */
        public async Task<bool> ConnectAsync()
        {
            // Connect Band
            // Verify that a MS Band is connected
            IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
            if (pairedBands.Length < 1)
            {
                return false; // Return in error if no Bands are connected
            }

            // Connect to Microsoft Band.
            using (bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]))
            {
                // Local Variable
                bool heartRateConsentGranted;

                // Ensure Heartrate Sensor Permissions
                // Ask for Permission if needed
                if (bandClient.SensorManager.HeartRate.GetCurrentUserConsent() == UserConsent.Granted)
                {
                    heartRateConsentGranted = true;
                }
                else
                {
                    heartRateConsentGranted = await bandClient.SensorManager.HeartRate.RequestUserConsentAsync();
                }
                // Return Permission status
                if (!heartRateConsentGranted)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }  

        /*
         * Check HeartRate Permissions
         * 
         * **Notes: This portion appears to work; however, when seperated methods are called,
         *      (startRead errors) errors out saying object has been removed--NEEDS WORK 
         */
        public async Task<bool> HeartRatePerm() {
            // Ensure MS Band is Connected
                if (bandClient==null) { await ConnectAsync(); }

            using (bandClient)
            {
                // Local Variable
                bool heartRateConsentGranted;

                // Ensure Heartrate Sensor Permissions
                // Ask for Permission if needed
                if (bandClient.SensorManager.HeartRate.GetCurrentUserConsent() == UserConsent.Granted)
                {
                    heartRateConsentGranted = true;
                }
                else
                {
                    heartRateConsentGranted = await bandClient.SensorManager.HeartRate.RequestUserConsentAsync();
                }
                // Return Permission status
                if (!heartRateConsentGranted)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /*
         * Read HeartRate
         * 
         * **Notes: This portion does not work. errors out saying object has been removed--NEEDS WORK 
         */
        public async Task<int> startRead(int RunTime, TextBlock OutputText)
        {
             using (bandClient)
            {
                // Subscribe to HeartRate data.
                bandClient.SensorManager.HeartRate.ReadingChanged += async (s, args) =>
                {
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        OutputText.Text = string.Format("{0}", args.SensorReading.HeartRate.ToString());
                    });
                };
                await bandClient.SensorManager.HeartRate.StartReadingsAsync();


                // Receive HeartRate data for a while, then stop the subscription.
                await Task.Delay(TimeSpan.FromSeconds(RunTime));
                await bandClient.SensorManager.HeartRate.StopReadingsAsync();

                OutputText.Text = string.Format("Finished Sampling");
                return 0;
            }
        }

        /*
         * Stop HeartRate
         * 
         * **Notes: Can't test until startRead works. -- MAY NEED WORK 
         */
        public async Task<int> stopRead()
        {
            using (bandClient)
            {
                await bandClient.SensorManager.HeartRate.StopReadingsAsync();
                return 0;
            }
        }
               
    }

    }
