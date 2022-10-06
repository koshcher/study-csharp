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
using System.Runtime.Serialization;


namespace MulticastChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public string userName;
        public MainWindow()
        {
            InitializeComponent();
        }
        MulticastUdpClient udpClientWrapper;

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {

      userName = userNameText.Text;
      userNameText.IsReadOnly = true;
            int port = Int32.Parse(txtPort.Text);
            IPAddress multicastIPaddress = IPAddress.Parse(txtRemoteIP.Text);
            IPAddress localIPaddress = IPAddress.Any;

          
            udpClientWrapper = new MulticastUdpClient(multicastIPaddress, port, localIPaddress);
            udpClientWrapper.UdpMessageReceived += OnUdpMessageReceived;

            AddToLog("UDP Client started");
        }
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
           
            string msgString = String.Format("{0}|{1}|Message from ip: {2} text message: {3}",
              userName,
              DateTime.Now,
                GetLocalIPAddress(),
               txtMessage.Text);
            byte[] buffer = Encoding.Unicode.GetBytes(msgString);

            // Send
            udpClientWrapper.SendMulticast(buffer);
            AddToLog("Sent message: " + msgString);
        }
        
        void OnUdpMessageReceived(object sender, MulticastUdpClient.UdpMessageReceivedEventArgs e)
        {
            string receivedText = ASCIIEncoding.Unicode.GetString(e.Buffer);
            string user = receivedText.Substring(0, receivedText.IndexOf('|'));
            if(user != userName) {
              AddToLog("Received message: " + receivedText);
            }
        }

        void AddToLog(string s)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                txtLog.Text += Environment.NewLine;
                txtLog.Text += s;
            }), null);
        }

    void AddUserToLog(string s)
    {
      Dispatcher.BeginInvoke((Action)(() =>
      {
        usersLog.Text += Environment.NewLine;
        usersLog.Text += s;
      }), null);
    }

    public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
    }
}
