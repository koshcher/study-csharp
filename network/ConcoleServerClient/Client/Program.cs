using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect("127.0.0.1", 4006);
                byte[] data = new byte[1024];
                StringBuilder response = new StringBuilder();
                NetworkStream stream = client.GetStream();
                do
                {
                    int bytes = stream.Read(data, 0, data.Length);
                    response.Append(Encoding.UTF8.GetString(data, 0, bytes));
                } while (stream.DataAvailable);

                Console.WriteLine(response.ToString());
                string message = string.Format("At {0} from {1} get string: Hello client!", DateTime.Now.ToString(), "127.0.0.1");
                //data = new byte[256];
                data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Message sended");
                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();

        }
    }
}
