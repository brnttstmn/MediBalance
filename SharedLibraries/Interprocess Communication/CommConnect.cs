using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Interprocess_Communication
{
    public static class CommConnect
    {
        public static void batchConnect(Comm[] commArray)
        {
            System.Threading.Tasks.Parallel.ForEach(commArray, comm =>
            {
                if (comm.commType == typeof(Pipe))
                {
                    ((Pipe)comm).start();
                }
                else if (comm.commType == typeof(TCP))
                {
                    ((TCP)comm).connectTcp();
                }
            });
        }
    }
}
