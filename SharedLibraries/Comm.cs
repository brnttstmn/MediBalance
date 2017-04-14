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
        protected Type type;
        protected Thread readWriteThread;
        protected bool streamActive;
        protected bool threadActive;
        protected string commName;

        public Type commType { get { return type; } }
        public Thread thread { get { return readWriteThread; } }
        public string name { get { return commName; } }

        public Comm()
        {

        }

        public void startThread(ThreadStart start)
        {
            readWriteThread = new Thread(start);
            threadActive = true;
        }
        public void stopThread(ThreadStart thread)
        {
            readWriteThread.Abort();
            threadActive = false;
        }

        public static void disconnectComm(Comm[] commlist)
        {
            Parallel.ForEach(commlist, comm => {
                if(comm.type == typeof(Pipe)) { ((Pipe)comm).stop(); }
                else if (comm.type == typeof(TCP)) { ((TCP)comm).stop(); }
            });
        }
        public static void disconnectComm(List<Comm> commlist)
        {
            Parallel.ForEach(commlist, comm => {
                if (comm.type == typeof(Pipe)) { ((Pipe)comm).stop(); }
                else if (comm.type == typeof(TCP)) { ((TCP)comm).stop(); }
            });
        }
        public static void connectComm(Comm[] commlist)
        {
            Parallel.ForEach(commlist, comm => {
                if (comm.type == typeof(Pipe)) { ((Pipe)comm).start(); }
                else if (comm.type == typeof(TCP)) { ((TCP)comm).start(); }
            });
        }
        public static void connectComm(List<Comm> commlist)
        {
            Parallel.ForEach(commlist, comm => {
                if (comm.type == typeof(Pipe)) { ((Pipe)comm).start(); }
                else if (comm.type == typeof(TCP)) { ((TCP)comm).start(); }
            });
        }

    }
}
