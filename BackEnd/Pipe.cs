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
        public NamedPipeClientStream client = null;
        public NamedPipeServerStream server = null;
        public string name;
        public string path;
        public StreamReader read;
        public StreamWriter write;

        public Pipe(string name, NamedPipeClientStream pipe, string path)
        {
            this.client = pipe;
            this.path = path;
            this.name = name;
        }

        public Pipe(string name, NamedPipeServerStream pipe, string path)
        {
            this.server = pipe;
            this.path = path;
            this.name = name;
        }

        public void start_client()
        {
            // Waiting for Connection
            Console.WriteLine("Waiting for connection...");

            //server.WaitForConnection();
            this.client.Connect();
            Console.WriteLine("Connected.");

            start_reader(this.client);
            start_writer(this.client);

        }

        public void start_server()
        {
            // Waiting for Connection
            Console.WriteLine("Waiting for connection...");

            //server.WaitForConnection();
            server.WaitForConnection();
            Console.WriteLine("Connected.");

            start_reader(this.server);
            start_writer(this.server);

        }

        public void stop_client(NamedPipeClientStream client)
        {
            //Close pipe client        
            client.Close();
        }

        public void start_reader(NamedPipeClientStream client)
        {
            // Instantiate Stream reader and Writers
            this.read = new StreamReader(client);
        }

        public void start_writer(NamedPipeClientStream client)
        {
            // Instantiate Stream reader and Writers
            this.write = new StreamWriter(client) { AutoFlush = true };
        }

        public void start_reader(NamedPipeServerStream server)
        {
            // Instantiate Stream reader and Writers
            this.read = new StreamReader(server);
        }

        public void start_writer(NamedPipeServerStream server)
        {
            // Instantiate Stream reader and Writers
            this.write = new StreamWriter(server) { AutoFlush = true };
        }

        public void sendcommand(string command)
        {
            this.write.WriteLine(command);
            Console.WriteLine("Start sent");
        }

    }
}
