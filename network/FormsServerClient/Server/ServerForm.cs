using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
  public partial class ServerForm : Form
  {
    private Dictionary<string, string> commands = new Dictionary<string, string>();
    private IPEndPoint ep;
    private IPAddress ip;
    private int port;
    private string[] randAnswer = { "Wow!", "Amazing!", "Good luck", "Let's drink", "Yes", "Unbelivable" };
    private TcpListener server;

    public ServerForm()
    {
      InitializeComponent();
    }

    private void sendBtn_Click(object sender, EventArgs e)
    {
      Task.Run(() => Start());
    }

    private string ServerMessage(string clientMessage)
    {
      if (clientMessage.Contains("hello") || clientMessage.Contains("hi"))
      {
        return "Hi! How are you?";
      }
      else if (clientMessage.Contains("how are you"))
      {
        return "I am fine.";
      }
      else if (clientMessage.Contains("what") && clientMessage.Contains("doing"))
      {
        return "I am coding myself";
      }
      return randAnswer[new Random().Next(0, randAnswer.Length)];
    }

    private void Start()
    {
      if (int.TryParse(portTextBox.Text, out port))
      {
        ip = IPAddress.Parse(ipTextBox.Text);
        ep = new IPEndPoint(ip, port);
        server = new TcpListener(ep);

        try
        {
          server.Start();
          while (true)
          {
            messageTextBox.Invoke((MethodInvoker)delegate ()
            {
              messageTextBox.Text += ("Wait for connection...\n");
            });
            TcpClient client = server.AcceptTcpClient();
            messageTextBox.Invoke((MethodInvoker)delegate ()
            {
              messageTextBox.Text += "Client connected\n";
            });

            NetworkStream stream = client.GetStream();
            byte[] data = new byte[256];

            // read
            StringBuilder response = new StringBuilder();
            do
            {
              int bytes = stream.Read(data, 0, data.Length);
              response.Append(Encoding.UTF8.GetString(data, 0, bytes));
            } while (stream.DataAvailable);

            string clientMessage = response.ToString().ToLower();
            if (clientMessage.Contains("bye"))
            {
              stream.Close();
              client.Close();
              server.Stop();
              messageTextBox.Invoke((MethodInvoker)delegate ()
              {
                messageTextBox.Clear();
              });
              break;
            }
            else
            {
              messageTextBox.Invoke((MethodInvoker)delegate ()
              {
                messageTextBox.Text = response.ToString() + "\n";
              });
            }

            // send
            data = Encoding.UTF8.GetBytes(string.Format("At {0} from {1} get:\n{2}\n",
                DateTime.Now.ToShortTimeString(), ip, ServerMessage(clientMessage)));
            stream.Write(data, 0, data.Length);
            messageTextBox.Invoke((MethodInvoker)delegate ()
            {
              messageTextBox.Text += "Message sended\n";
            });

            stream.Close();
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
          server.Stop();
        }
      }
    }
  }
}