using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Client
{
  public partial class ClientForm : Form
  {
    private int port;
    private string ip;
    private TcpClient client;
    private NetworkStream stream;

    public ClientForm()
    {
      InitializeComponent();
    }

    private void sendBtn_Click(object sender, EventArgs e)
    {
      ip = ipTextBox.Text;
      if (int.TryParse(portTextBox.Text, out port))
      {
        string str = inputTextBox.Text;
        try
        {
          client = new TcpClient();
          client.Connect(ip, port);
          stream = client.GetStream();

          // send
          string message = string.Format("At {0} from {1} get:\n{2}",
              DateTime.Now.ToShortTimeString(), ip, str);
          byte[] data = Encoding.UTF8.GetBytes(message);
          stream.Write(data, 0, data.Length);

          // read
          StringBuilder response = new StringBuilder();
          do
          {
            int bytes = stream.Read(data, 0, data.Length);
            response.Append(Encoding.UTF8.GetString(data, 0, bytes));
          } while (stream.DataAvailable);
          messageTextBox.Text += response.ToString() + '\n';

          // close
          stream.Close();
          client.Close();
        }
        catch (SocketException ex)
        {
          messageTextBox.Text = (ex.Message);
        }
        catch (Exception ex)
        {
          messageTextBox.Text = (ex.Message);
        }
      }
      else
      {
        MessageBox.Show("Port is not a number");
      }
    }
  }
}