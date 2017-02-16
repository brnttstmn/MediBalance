using System;
using System.Collections;
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
        public Task<int> run(int dur, List<int> sample, BitArray control, Dictionary<string, int> map)
        {

            return Task.FromResult(0);
        }

        public Task<int> heartRate(int dur, List<int> sample)
        {
            Random random = new Random();

            for (int i = 0; i < dur; i++)
            {                
                int randomNumber = random.Next(40, 100);
                sample.Add(randomNumber);
            }
            return Task.FromResult(0);
        }

        public Task<int> gsr(int dur, List<int> sample)
        {
            Random random = new Random();

            for (int i = 0; i < dur; i++)
            {
                int randomNumber = random.Next(40, 100);
                sample.Add(randomNumber);
            }
            return Task.FromResult(0);

        }

        public Task<int> ls(int dur, List<int> sample)
        {
            Random random = new Random();

            for (int i = 0; i < dur; i++)
            {
                int randomNumber = random.Next(40, 100);
                sample.Add(randomNumber);
            }
            return Task.FromResult(0);

        }

        public Task<int> acc(int dur, List<int> sample)
        {
            Random random = new Random();

            for (int i = 0; i < dur; i++)
            {
                int randomNumber = random.Next(40, 100);
                sample.Add(randomNumber);
            }
            return Task.FromResult(0);

        }

        public Task<int> alt(int dur, List<int> sample)
        {
            Random random = new Random();

            for (int i = 0; i < dur; i++)
            {
                int randomNumber = random.Next(40, 100);
                sample.Add(randomNumber);
            }
            return Task.FromResult(0);

        }

        public Task<int> barometer(int dur, List<int> sample)
        {
            Random random = new Random();

            for (int i = 0; i < dur; i++)
            {
                int randomNumber = random.Next(40, 100);
                sample.Add(randomNumber);
            }
            return Task.FromResult(0);

        }

    }
}
