using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

namespace WindowsFormsApp9
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           

        }

        TcpClient client;
        NetworkStream stream;

        private void button1_Click(object sender, EventArgs e)
        {
            string str = textBox1.Text;
            try
            {
                TcpClient client = new TcpClient();
                client.Connect("127.0.0.1", 4006);
                byte[] data = new byte[256];
                NetworkStream stream = client.GetStream();
                StringBuilder response = new StringBuilder();
              
                do
                {
                    int bytes = stream.Read(data, 0, data.Length);
                    response.Append(Encoding.UTF8.GetString(data, 0, bytes));
                } while (stream.DataAvailable);

                textBox2.Text=(response.ToString());
                string message = String.Format("В {0} от {1} {2}", DateTime.Now.ToShortTimeString(), "127.0.0.1", str);
                data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
                stream.Close();
                client.Close();
            }
            catch (SocketException ex)
            {
                textBox2.Text = (ex.Message);
            }
            catch (Exception ex)
            {

                textBox2.Text=(ex.Message);
            }
           

        }

       
    }
}
