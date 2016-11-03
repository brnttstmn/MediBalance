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

namespace MediBalance
{
    class MSBand2
    {
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
        public async Task<int> everything(int RunTime, TextBlock OutputText)
        {
            OutputText.Text = "Connecting...";
            OutputText.Visibility = Visibility.Visible;
            try
            {
                // Get the list of Microsoft Bands paired to the phone.
                IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
                if (pairedBands.Length < 1)
                {
                    OutputText.Text = "Microsoft Band cannot be found. Check Connection";
                    return -1;
                }
                // Connect to Microsoft Band.
                using (IBandClient bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]))
                {
                    bool heartRateConsentGranted;

                    // Check whether the user has granted access to the HeartRate sensor.
                    if (bandClient.SensorManager.HeartRate.GetCurrentUserConsent() == UserConsent.Granted)
                    {
                        heartRateConsentGranted = true;
                    }
                    else
                    {
                        heartRateConsentGranted = await bandClient.SensorManager.HeartRate.RequestUserConsentAsync();
                    }

                    if (!heartRateConsentGranted)
                    {
                        OutputText.Text = "Access to the heart rate sensor is denied.";
                        return -1;
                    }
                    else
                    {
                        OutputText.Text = "Connected!";
                        OutputText.Text = "Collecting Samples ...";

                        // Subscribe to HeartRate data.
                        bandClient.SensorManager.AmbientLight.ReadingChanged += async (s, args) =>
                        {
                            await Windows.ApplicationModel.Core.CoreApplication.MainView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                            {
                                OutputText.Text = string.Format("{0}", args.SensorReading.Brightness.ToString());
                            });
                        };
                        await bandClient.SensorManager.AmbientLight.StartReadingsAsync();


                        // Receive HeartRate data for a while, then stop the subscription.
                        await Task.Delay(TimeSpan.FromSeconds(RunTime));
                        await bandClient.SensorManager.AmbientLight.StopReadingsAsync();

                        OutputText.Text = string.Format("Finished Sampling");
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                OutputText.Text = ex.ToString();
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
        public async Task<int> startRead()
        {
            // Ensure MS Band is Connected
            if (bandClient == null) { await ConnectAsync(); }

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
                    return -1;
                }
                else
                {
                    await bandClient.SensorManager.HeartRate.StartReadingsAsync();
                    return 0;
                }
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
