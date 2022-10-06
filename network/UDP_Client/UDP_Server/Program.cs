using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDP_Server
{
  class Program
  {
    static int localPort; //порт для приема сообщений
    static Socket listeningSocket; //Сокет

    static List<IPEndPoint> clients = new List<IPEndPoint>();
    static void Main(string[] args)
    {
      Console.WriteLine("UDP SERVER");
      Console.WriteLine("Enter port number for getting mesages");
      localPort = Int32.Parse(Console.ReadLine());
      Console.WriteLine();

      try
      {
        listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);//Створення сокета
        Task listeningTask = new Task(Listen);//Стоврення потоку для отримання повідомлень
        listeningTask.Start();//Запуск потоку
        listeningTask.Wait();//Очікуєм зупинки потоку
      }
      catch (Exception ex)
      {

        Console.WriteLine(ex.Message);
      }
    }
    private static void Listen()
    {
      IPEndPoint localIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), localPort);
      listeningSocket.Bind(localIP);
      try
      {
        while (true)
        {
          StringBuilder builder = new StringBuilder();
          int bytes = 0;
          byte[] data = new byte[256];
          EndPoint clientIp = new IPEndPoint(IPAddress.Any, 0);//адресс клієнта
          do
          {
            bytes = listeningSocket.ReceiveFrom(data, ref clientIp);
            builder.Append(Encoding.Unicode.GetString(data, 0, bytes));

          } while (listeningSocket.Available > 0);
          IPEndPoint clientFullIp = clientIp as IPEndPoint;
          Console.WriteLine("{0}:{1} - {2}", clientFullIp.Address.ToString(), clientFullIp.Port, builder.ToString());

          bool isClient = true;

          for (int i = 0; i < clients.Count; i++)
          {
            if (clients[i].ToString() == clientFullIp.ToString())
              isClient = false;
                //clients.Add(clientFullIp);
          }
          if (isClient)
            clients.Add(clientFullIp);

          SendMessagesToAllClients(builder.ToString(), clientFullIp.Port.ToString());
        }
      }
      catch (Exception ex)
      {

        Console.WriteLine(ex.Message);
      }
      finally
      {
        Close();
      }
    }
    private static void SendMessagesToAllClients(string message, string port)
    {
      byte[] data = Encoding.Unicode.GetBytes(message);

      for (int i = 0; i < clients.Count; i++)
      {
        if (clients[i].Port.ToString() != port)
          listeningSocket.SendTo(data, clients[i]);
      }
    }
    private static void Close()
    {
      if (listeningSocket != null)
      {
        listeningSocket.Shutdown(SocketShutdown.Both);
        listeningSocket.Close();
        listeningSocket = null;
      }
      Console.WriteLine("Server stoped");
    }
  }
}
