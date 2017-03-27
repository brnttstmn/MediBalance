using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace BackEnd
{
    class Pipe
    {
        // Private
        private NamedPipeClientStream client = null;
        private NamedPipeServerStream server = null;
        private StreamReader streamRead;
        private StreamWriter streamWrite;
        private Thread readWriteThread;

        // Public
        public bool isClient { get; }
        public string name { get; }
        public string path { get; }

        // Accessors
        public StreamReader read { get { return streamRead; } }
        public StreamWriter write { get { return streamWrite; } }
        public Thread thread { get { return readWriteThread; } }

        // Constructors
        public Pipe(string name, bool isClient, string path)
        {
            this.isClient = isClient;
            this.path = path;
            this.name = name;
        }
        public Pipe(string name, bool isClient)
        {
            this.isClient = isClient;
            this.name = name;
        }

        // Public Methods
        public void start()
        {
            if (!isClient)
            {
                startServer();
            }
            else
            {
                startClient();
            }
        }
        public void stop()
        {
            if (!isClient) { server.Dispose(); }
            else { client.Dispose(); }
        }
        public void sendcommand(string command)
        {
            this.write.WriteLine(command);
            Console.WriteLine("sent: " + command);
        }
        public void startThread(ThreadStart start)
        {
            readWriteThread = new Thread(start);
        }
        public void stopThread(ThreadStart thread)
        {
            readWriteThread.Abort();
        }

        //Private Methods
        /// <summary>
        /// Start client
        /// </summary>
        private void startClient()
        {
            Console.WriteLine("Connecting to " + name + "...");
            client = new NamedPipeClientStream(".", name, PipeDirection.InOut);
            client.Connect();
            Console.WriteLine("Connected to " + name + ".");
            streamRead = new StreamReader(client);
            streamWrite = new StreamWriter(client) { AutoFlush = true };
        }
        private void startServer()
        {
            Console.WriteLine("Connecting to " + name + "...");
            server = new NamedPipeServerStream(name, PipeDirection.InOut);
            server.WaitForConnection();
            Console.WriteLine("Connected to " + name + ".");
            streamRead = new StreamReader(server);
            streamWrite = new StreamWriter(server) { AutoFlush = true };
        }
    }
}
