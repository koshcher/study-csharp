using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
  public partial class ServerForm : Form
  {
     int localPort; //порт для приема сообщений
     Socket listeningSocket; // Сокет
     List<IPEndPoint> clients = new List<IPEndPoint>();

    public ServerForm()
    {
      InitializeComponent();
    }

    private void startBtn_Click(object sender, EventArgs e)
    {
      if(int.TryParse(portTextBox.Text, out localPort))
      {
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
    }

    private void Listen()
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
          messageTextBox.Invoke((MethodInvoker)delegate ()
          {
            messageTextBox.Text += string.Format("{0}:{1} - {2}", clientFullIp.Address.ToString(), clientFullIp.Port, builder.ToString());
          });
         
          bool isClient = true;

          for (int i = 0; i < clients.Count; i++)
          {
            if (clients[i].ToString() == clientFullIp.ToString())
              isClient = false;
          }
          if (isClient)
            clients.Add(clientFullIp);

          SendMessagesToAllClients(builder.ToString(), clientFullIp.Port.ToString());
        }
      }
      catch (Exception ex)
      {
        messageTextBox.Invoke((MethodInvoker)delegate ()
        {
          messageTextBox.Text = ex.Message;
        });
      }
      finally
      {
        CloseServer();
      }
    }

    private void SendMessagesToAllClients(string message, string port)
    {
      byte[] data = Encoding.Unicode.GetBytes(message);

      for (int i = 0; i < clients.Count; i++)
      {
        if (clients[i].Port.ToString() != port)
          listeningSocket.SendTo(data, clients[i]);
      }
    }
    private void CloseServer()
    {
      if (listeningSocket != null)
      {
        listeningSocket.Shutdown(SocketShutdown.Both);
        listeningSocket.Close();
        listeningSocket = null;
      }
      messageTextBox.Invoke((MethodInvoker)delegate ()
      {
        messageTextBox.Text = "Server stoped";
      });
    }
  }
}
