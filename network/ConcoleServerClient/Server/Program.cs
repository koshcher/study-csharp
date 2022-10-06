using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint ep = new IPEndPoint(ip, 4006);

            TcpListener server = new TcpListener(ep);
            try
            {
                server.Start();
                Console.WriteLine("Wait connecting...");
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Client connected, wait request");
                NetworkStream stream = client.GetStream();
                while (true)
                {
                    byte[] data = new byte[1024];
                    /*
                    string message = String.Format("At {0} from {1} get string: Hello client!", DateTime.Now.ToString(), ip);
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("Message sended");
                    */
                    StringBuilder response = new StringBuilder();
                    do
                    {
                        int bytes = stream.Read(data, 0, data.Length);
                        response.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    } while (stream.DataAvailable);
                    Console.WriteLine(response.ToString());
                }
                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                server.Stop();
            }
        }
    }
    
}
