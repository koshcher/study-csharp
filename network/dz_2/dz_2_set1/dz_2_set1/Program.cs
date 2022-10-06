using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace dz_2_set1
{
    class Program
    {

        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint ep = new IPEndPoint(ip, 4006);

            TcpListener server = new TcpListener(ep);
            byte[] data = new byte[256];
            try
            {
                server.Start();
                while (true)
                {
                   
                    Console.WriteLine("Ожидаем подключений....");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Клиент Подключился ждем запрос");
                    NetworkStream stream = client.GetStream();
                    StringBuilder response = new StringBuilder();
               
                    do
                    {
                        int bytes = stream.Read(data, 0, data.Length);
                        response.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    } while (stream.DataAvailable);
                    Console.WriteLine(response.ToString());
                    //////////////
                    string str = response.ToString();
                    string first = str.Split()[0];
                    string second = str.Split()[1];
                    string message = "";
                    switch (first)
                    {
                        case "USD":
                          if(second=="EURO")
                            {
                                message ="1 USD це 0.93 EURO";
                            }  
                          else if (second == "UAH")
                            {
                                message ="1 USD це 29.5 UAH";
                            }
                            else if (second == "PLN")
                            {
                                message = "1 USD це 4.3 PLN";
                            }
                            else if(second=="USD")
                            {
                                message = "1 USD це 1 USD";
                            }
                           else
                            {
                                message = "Некоректні дані!";
                            }
                            break;

                        case "EURO":
                            if (second == "USD")
                            {
                                message = "1 EURO це 1.07 USD";
                            }
                            else if (second == "UAH")
                            {
                                message = "1 EURO це 31.6 UAH";
                            }
                            else if (second == "PLN")
                            {
                                message = "1 EURO це 4.6 PLN";
                            }
                            else if(second=="EURO")
                            {
                                message = "1 EURO це 1 EURO";
                            }
                             else
                            {
                                message = "Некоректні дані!";
                            }
                            break;

                        case "UAH":
                            if (second == "USD")
                            {
                                message = "1 UAH це 0.316 USD";
                            }
                            else if (second == "EURO")
                            {
                                message = "1 UAH це 0.0339 EURO";
                            }
                            else if (second == "PLN")
                            {
                                message = "1 UAH це 0.145 PLN";
                            }
                            else if(second=="UAH")
                            {
                                message = "1 UAH це 1 UAH";
                            }

                             else
                            {
                                message = "Некоректні дані!";
                            }
                            break;

                        case "PLN":
                            if (second == "USD")
                            {
                                message = "1 PLN це 0.233 USD";
                            }
                            else if (second == "EURO")
                            {
                                message = "1 PLN це 0.217 EURO";
                            }
                            else if (second == "UAH")
                            {
                                message = "1 PLN це 6,88 UAH";
                            }
                            else if(second=="PLN")
                            {
                                message = "1 PLN це 1 PLN";
                            }
                            else
                            {
                                message = "Некоректні дані!";
                            }
                            break;
                        default:
                            message = "Некоректні дані!";
                            break;
                    }
                    data = Encoding.UTF8.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("Сообщение отправлено");
                    stream.Close();
                    client.Close();
                }
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
