using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharedLibraries
{
    public class Comm
    {
        // Protected
        protected bool active;
        protected Thread readWriteThread;
        protected string identity;
        protected Type CommType;

        public Type commType { get { return CommType; } }

        // Public
        public Thread thread { get { return readWriteThread; } }
        public bool isStarted { get { return active; } }
        public string name { get { return identity; } }

        // Constructor
        public Comm() { }

        // Public Methods
        public void startThread(ThreadStart start)
        {
            readWriteThread = new Thread(start);
        }
        public void stopThread(ThreadStart thread)
        {
            readWriteThread.Abort();
        }
    }


}
