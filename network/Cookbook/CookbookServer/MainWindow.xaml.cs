using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CookbookServer
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    int localPort;
    string localIp;
    bool run = false;
    Socket listeningSocket;
   // List<IPEndPoint> clients = new List<IPEndPoint>();
    Dictionary<IPEndPoint, DateTime> clients = new Dictionary<IPEndPoint, DateTime>();
    Dictionary<string, string[]> recipes = new Dictionary<string,string[]>(); // array because write once 
    Logger logger = new Logger();

    public MainWindow()
    {
      InitializeComponent();
      recipes.Add("sandwich", new string[] { "bread", "cheese", "sausage", "butter"});
      recipes.Add("omelet", new string[] { "egg", "milk", "solt" });
      recipes.Add("borsch", new string[] { "potato", "tomato", "cabbage", "carrot" });
    }

    private void StartBtn_Click(object sender, RoutedEventArgs e)
    {
      if (int.TryParse(localPortTextBox.Text, out localPort))
      {
        run = true;
        localIp = ipTextBox.Text;
        localPortTextBox.IsReadOnly = true;
        ipTextBox.IsReadOnly = true;

      //  startBtn.IsEnabled = false;
        try
        {
          listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
          Task listenongTask = new Task(Listen);

          listenongTask.Start();
          startBtn.IsEnabled = false;
          //listenongTask.Wait();
        }
        catch (Exception ex)
        {
          localPortTextBox.IsReadOnly = false;
          ipTextBox.IsReadOnly = false;
          startBtn.IsEnabled = true;
          MessageBox.Show(ex.Message);
          CloseConnection();
        }
      }
    }
    private void Listen()
    {
      IPAddress ip;
      if (IPAddress.TryParse(localIp, out ip))
      {
        IPEndPoint localIp = new IPEndPoint(ip, localPort);
        listeningSocket.Bind(localIp);
        try
        {
          while (run)
          {
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            byte[] data = new byte[1024];
            EndPoint clientIp = new IPEndPoint(IPAddress.Any, 0);

            do
            {
              bytes = listeningSocket.ReceiveFrom(data, ref clientIp);
              builder.Append(Encoding.Unicode.GetString(data, 0, bytes));

            } while (listeningSocket.Available > 0);
            IPEndPoint clientFullIp = (IPEndPoint)clientIp;
            string message = builder.ToString();

            AddToOutput(string.Format("{0}:{1} - {2}", clientFullIp.Address.ToString(), clientFullIp.Port, message));
            logger.LogRequest(message, clientFullIp);

            ManageClient(clientFullIp);

            if(message == "__disconnect__")
            {
              DisconnectClient(clientFullIp);
            }
            else {
              SendData(GetRecipes(builder.ToString()), clientFullIp.Port);
            }
          }
        }
        catch (SocketException socketEx)
        {
          if (socketEx.ErrorCode != 10004)
            MessageBox.Show(socketEx.Message + socketEx.ErrorCode);
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message);
        }
        finally
        {
          CloseConnection();
        }
      }
    }

    private void DisconnectClient(IPEndPoint clientFullIp)
    {
      clients.Remove(clientFullIp);
      logger.LogDisconnect(clientFullIp);
    }

    private void ManageClient(IPEndPoint clientFullIp)
    {
      if(clients.Count >= 5) 
      {
        foreach (var client in clients)
        {
          // if connected more than 5 min
          if ((DateTime.Now - client.Value).TotalMinutes > 5)
          {
            DisconnectClient(client.Key);
          }
        }
      }
      if(clients.Count < 5)
      {
        bool isClient = true;
        foreach(var client in clients)
        {
          if (client.Key.ToString() == clientFullIp.ToString())
          {
            isClient = false;
          }
        }
        if (isClient)
        {
          clients.Add(clientFullIp, DateTime.Now);
          logger.LogConnection(clientFullIp);
        }
      } 
      else
      {
        if (!clients.ContainsKey(clientFullIp)) {
          SendError(clientFullIp);
        }
      }
    }


    private void SendError(IPEndPoint clientFullIp)
    {
      byte[] data = Encoding.Unicode.GetBytes("Too much clients are connected to server! Try later!");
      listeningSocket.SendTo(data, clientFullIp);
    }

    private void SendData(string message, int port)
    {
      byte[] data = Encoding.Unicode.GetBytes(message);

      foreach(var client in clients)
      {
        if (client.Key.Port == port) // send only to one who asked
        {
          listeningSocket.SendTo(data, client.Key);
          clients[client.Key] = DateTime.Now; // update time
        }
      }
    }

    private void CloseConnection()
    {
      if (listeningSocket != null)
      {
        listeningSocket.Shutdown(SocketShutdown.Both);
        listeningSocket.Close();
        listeningSocket = null;
      }
    }

    private string GetRecipes(string message)
    {
      HashSet<string> res = new HashSet<string>(); // list because ofter write
      string[] components = message.Split("\u002C ");
      foreach (KeyValuePair<string, string[]> recipe in recipes)
      {
        foreach (string component in components)
        {
          if (recipe.Value.Contains(component))
            res.Add(recipe.Key);
        }
      }
      return string.Join(", ", res);
    }

    private void AddToOutput(string message)
    {
      Dispatcher.BeginInvoke(() =>
      {
        //messageTextBlock.Text += Environment.NewLine;
        outputTextBox.Text += string.Format("{0}\n", message);
      }, null);
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      run = false;
      CloseConnection();
    }
  }
}
