using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MediBalance.TestVectors
{
    // MSBand Test Vector functions
    class TestBand
    {
        public void heartRate(int dur, ref int hr)
        {
            int[] arr = { 50, 51, 51, 50, 50, 51, 52, 51, 50, 50, 50, 51, 51, 50, 50, 51, 52, 51, 50, 50, 50, 51, 51, 50, 50, 51, 52, 51, 50, 50 };

            for (int i = 0; i < dur; i++)
            {
                hr = arr[i];
            }

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
