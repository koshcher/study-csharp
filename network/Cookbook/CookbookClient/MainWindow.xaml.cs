using System;
using System.Collections.Generic;
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

namespace CookbookClient
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    int remotePort;
    int localPort;
    string remoteIp;
    bool run = false;
    Socket listeningSocket;

    Random rnd = new Random();
    Task listenongTask;

    public MainWindow()
    {
      InitializeComponent();
      int randLocalPort; // random port from aviable
      do
      {
        randLocalPort = rnd.Next(2000, 49000);
      } while (randLocalPort == 5001); // 5001 - server port
      localPortTextBox.Text = randLocalPort.ToString();
    }

    private void StartBtn_Click(object sender, RoutedEventArgs e)
    {
      if (int.TryParse(localPortTextBox.Text, out localPort) && int.TryParse(remotePortTextBox.Text, out remotePort))
      {
        run = true;
        localPortTextBox.IsReadOnly = true;
        remotePortTextBox.IsReadOnly = true;
        remoteIpTextBox.IsReadOnly = true;
        remoteIp = remoteIpTextBox.Text;
        startBtn.IsEnabled = false;
        DisconnectBtn.IsEnabled = true;
        try
        {
          listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
          listenongTask = new Task(Listen);
          listenongTask.Start();
        }
        catch (Exception ex)
        {
          startBtn.IsEnabled = true;
          DisconnectBtn.IsEnabled = false;
          MessageBox.Show(ex.Message);
        }
      }
    }

    private void SendBtn_Click(object sender, RoutedEventArgs e)
    {
      if(run)
      {
        try
        {
          SendData(inputTextBox.Text, remoteIpTextBox.Text, remotePort);
        } 
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message);
        }
      }
    }

    private void DisconnectBtn_Click(object sender, RoutedEventArgs e)
    {
      if (run)
      {
        try
        {
          SendData("__disconnect__", remoteIpTextBox.Text, remotePort);
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

    private void Listen()
    {
      try
      {
        IPEndPoint localIP = new IPEndPoint(IPAddress.Parse(remoteIp), localPort);
        listeningSocket.Bind(localIP);

        while (run)
        {
          StringBuilder builder = new StringBuilder();
          int bytes = 0; 
          byte[] data = new byte[256];

          // adress fromm where get
          EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);

          do
          {
            bytes = listeningSocket.ReceiveFrom(data, ref remoteIp);
            builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
          }
          while (listeningSocket.Available > 0);

          IPEndPoint remoteFullIp = (IPEndPoint)remoteIp;


          // output
          AddToOutput(builder.ToString());
        }
      }
      catch (SocketException socketEx)
      {
        if(socketEx.ErrorCode != 10004)
          MessageBox.Show(socketEx.Message + socketEx.ErrorCode);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message + ex.GetType().ToString());
      }
      finally
      {
        CloseConnection();
      }
    }

    private void SendData(string message, string ip, int port)
    {
      try
      {
        // where to send
        IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse(ip), port);
        byte[] data = Encoding.Unicode.GetBytes(message); // message in bytes
        listeningSocket.SendTo(data, remotePoint);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private void CloseConnection()
    {
      if (listeningSocket != null)
      {
        listeningSocket.Shutdown(SocketShutdown.Both);
        listeningSocket.Close();
        listeningSocket = null;
        startBtn.IsEnabled = true;
        DisconnectBtn.IsEnabled = false;
      }
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
      CloseConnection();
    }
  }
}
