using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndUIRedux
{
    class Pipe
    {
        // Private
        private NamedPipeClientStream client = null;
        private NamedPipeServerStream server = null;
        private StreamReader streamRead;
        private StreamWriter streamWrite;

        // Public
        public bool isClient { get; }
        public string name { get; }

        // Accessors
        public StreamReader read { get { return streamRead; } }
        public StreamWriter write { get { return streamWrite; } }

        // Constructors
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
            try
            {
                if (!isClient) { server.Dispose(); }
                else { client.Dispose(); }
            }
            catch (NullReferenceException) { }
        }
        public void sendcommand(string command)
        {
            this.write.WriteLine(command);
            Console.WriteLine("sent: " + command);
        }

        //Private Methods
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
