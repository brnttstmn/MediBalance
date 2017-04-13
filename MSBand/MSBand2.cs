using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        private IBandClient bandClient = null;
        private static bool tcpConnected;

        /*
        * TSK Everything(int time, TextBlock OutputText):
        *  This method takes in an interger time and a textblock. The method will run for the given time
        *  and output the results live in the textbox. 
        *  
        *  **Note: Currently only running one sensor at a time (It can run all but not sure how we should output it...)
        * 
        */
        public async Task<int> everything(int RunTime, List<string> samples, BitArray control, Dictionary<string, int> map, TextBlock OutputText, Tcp_Client tcpClient)
        {
            tcpConnected = true;
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

                    if (control[map["hr"]])
                    {
                        // Subscribe to HeartRate data.
                        bandClient.SensorManager.HeartRate.ReadingChanged += async (s, args) =>
                        {
                            await Windows.ApplicationModel.Core.CoreApplication.MainView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                            {
                                string data_string = string.Format("{0},Heartrate,{1,0:D3};", DateTime.Now.ToString(timeFormat), args.SensorReading.HeartRate);

                                OutputText.Text += data_string;
                                samples.Add(data_string);
                                try { await tcpClient.send(data_string); }
                                catch (Exception) { tcpConnected = false; return; }
                                
                            });
                        };
                        await bandClient.SensorManager.HeartRate.StartReadingsAsync();
                    }
                    if (control[map["gsr"]])
                    {
                        // Subscribe to GSR data.
                        bandClient.SensorManager.Gsr.ReadingChanged += async (s, args) =>
                        {
                            await Windows.ApplicationModel.Core.CoreApplication.MainView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                            {
                                OutputText.Text += string.Format("\n{1},gsr,{0};", args.SensorReading.Resistance, DateTime.Now.ToString(timeFormat));
                                samples.Add(string.Format(string.Format("{1},gsr,{0};", args.SensorReading.Resistance, DateTime.Now.ToString(timeFormat))));
                                try { await tcpClient.send(string.Format("{1},gsr,{0,0:D9};", args.SensorReading.Resistance, DateTime.Now.ToString(timeFormat))); }
                                catch (Exception) { tcpConnected = false; return; }
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
                    while (tcpConnected) { }


                    // Shut off Sensors
                    await bandClient.SensorManager.HeartRate.StopReadingsAsync();
                    await bandClient.SensorManager.Gsr.StopReadingsAsync();
                    await bandClient.SensorManager.AmbientLight.StopReadingsAsync();
                    return 0;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }

}
