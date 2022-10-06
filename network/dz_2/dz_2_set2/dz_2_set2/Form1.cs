using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Windows.Forms;

namespace dz_2_set2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] first = { "USD", "EURO", "UAH", "PLN" };
            string[] second = { "USD", "EURO", "UAH", "PLN" };
            comboBox1.Items.AddRange(second);
            comboBox2.Items.AddRange(first);
        }

        TcpClient client;
        NetworkStream stream;
        public static bool flag = false;
        private void button1_Click(object sender, EventArgs e)
        {
            flag = true;
            string str = comboBox1.Text;
            string str1 = comboBox2.Text;
            if ((str != "") && (str1 != ""))
            {
                try
                {
                    client = new TcpClient();
                    client.Connect("127.0.0.1", 4006);
                    byte[] data = new byte[256];
                    stream = client.GetStream();
                    StringBuilder response = new StringBuilder();
                    string message = String.Format("{2} {3} в {0} от {1}", DateTime.Now.ToShortTimeString(), "127.0.0.1", str, str1);
                    data = Encoding.UTF8.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    do
                    {
                        int bytes = stream.Read(data, 0, data.Length);
                        response.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    } while (stream.DataAvailable);

                    textBox1.Text = (response.ToString());

                    stream.Close();
                    client.Close();
                }
                catch (SocketException ex)
                {
                    textBox1.Text = (ex.Message);
                }
                catch (Exception ex)
                {

                    textBox1.Text = (ex.Message);
                }


            }
           
        }

        
    }
    
}
