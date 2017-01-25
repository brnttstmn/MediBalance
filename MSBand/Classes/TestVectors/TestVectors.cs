using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MediBalance.TestVectors
{
    // MSBand Test Vector functions
    class TestBand
    {
        public Task<int> heartRate(int dur, List<int> hr)
        {
            Random random = new Random();

            for (int i = 0; i < dur; i++)
            {                
                int randomNumber = random.Next(40, 100);
                hr.Add(randomNumber);
            }
            return Task.FromResult(0);

        }

        public void galSkinResp(ref int a)
        {

        }

        public void ambientLight(ref int a)
        {

        }

        public void accelerometer(ref int a)
        {

        }

        public void altimeter(ref int a)
        {

        }

        public void barometer(ref int a)
        {

        }

    }
}
