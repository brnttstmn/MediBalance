using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd
{
    class Pipe
    {
        private NamedPipeClientStream client = null;
        private NamedPipeServerStream server = null;

        public object pipe;
        public string name;
        public string path;
        public StreamReader read;
        public StreamWriter write;

        // Constructors
        public Pipe(string name, NamedPipeClientStream pipe, string path)
        {
            this.client = pipe;
            this.pipe = pipe;
            this.path = path;
            this.name = name;
        }
        public Pipe(string name, NamedPipeServerStream pipe, string path)
        {
            this.server = pipe;
            this.pipe = pipe;
            this.path = path;
            this.name = name;
        }

        // Public Methods
        public void start(object pipe)
        {
            if (client == null) { start((NamedPipeServerStream)pipe); }
            else { start((NamedPipeClientStream)pipe); }
        }
        public void stop(object pipe)
        {
            if (client == null) { stop((NamedPipeServerStream)pipe); }
            else { stop((NamedPipeClientStream)pipe); }
        }
        public void sendcommand(string command)
        {
            this.write.WriteLine(command);
            Console.WriteLine("Start sent");
        }

        //Private Methods
        private void start(NamedPipeClientStream client)
        {
            // Waiting for Connection
            Console.WriteLine("Waiting for connection...");
            client.Connect();
            Console.WriteLine("Connected.");
            start_reader(client);
            start_writer(client);
        }
        private void start(NamedPipeServerStream server)
        {
            // Waiting for Connection
            Console.WriteLine("Waiting for connection...");
            server.WaitForConnection();
            Console.WriteLine("Connected.");
            start_reader(server);
            start_writer(server);
        }
        private void stop(NamedPipeClientStream client)
        {
            //Close pipe client        
            client.Close();
        }
        private void stop(NamedPipeServerStream server)
        {
            //Close pipe client        
            server.Close();
        }
        private void start_reader(NamedPipeClientStream client)
        {
            // Instantiate Stream reader and Writers
            this.read = new StreamReader(client);
        }
        private void start_writer(NamedPipeClientStream client)
        {
            // Instantiate Stream reader and Writers
            this.write = new StreamWriter(client) { AutoFlush = true };
        }
        private void start_reader(NamedPipeServerStream server)
        {
            // Instantiate Stream reader and Writers
            this.read = new StreamReader(server);
        }
        private void start_writer(NamedPipeServerStream server)
        {
            // Instantiate Stream reader and Writers
            this.write = new StreamWriter(server) { AutoFlush = true };
        }



    }
}
