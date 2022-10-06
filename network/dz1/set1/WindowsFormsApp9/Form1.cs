using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        TcpListener server = null;
        private void button1_Click(object sender, EventArgs e)
        {
            Task.Run(() => Start());
        
        }

        private void Start()
        {
            string[] mass = { "Hi!!!", "Klient", "Black", "Wite", "Grean", "Blue" };
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint ep = new IPEndPoint(ip, 4006);

            TcpListener server = new TcpListener(ep);
            try
            {
                server.Start();
                while (true)
                {
                    listBox1.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Text += ("Ожидаем подключений.... \n");

                    });
                    TcpClient client = server.AcceptTcpClient();
                    listBox1.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Text += "Клиент Подключился ждем запрос \n";

                    });
                    NetworkStream stream = client.GetStream();
                    string message = String.Format("В {0} от {1} {2}\n", DateTime.Now.ToShortTimeString(), ip, mass[new Random().Next(0, mass.Length)]);
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    listBox1.Invoke((MethodInvoker)delegate ()
                    {
                        listBox1.Text += "Сообщение отправлено \n";

                    });
                   
                    StringBuilder response = new StringBuilder();
                    do
                    {
                        int bytes = stream.Read(data, 0, data.Length);
                        response.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    } while (stream.DataAvailable);
                    string str = response.ToString();
                      str.ToLower();
                    if (str.ToLower().Substring(str.Length-4)==" bye")
                    {
                        stream.Close();
                        client.Close();
                        server.Stop();
                        listBox1.Invoke((MethodInvoker)delegate ()
                        {
                            listBox1.Clear();
                        });
                        break;
                    }

                    else
                    {
                        listBox1.Invoke((MethodInvoker)delegate ()
                        {
                            listBox1.Text = response.ToString()+"\n";

                        });

                        stream.Close();
                       
                    }
                }
            }
            catch (Exception ex)
            {
                textBox1.Invoke((MethodInvoker)delegate ()
                {
                    textBox1.Text = ex.Message;

                });
                
            }
            finally
            {
                server.Stop();
            }

        }
    }
    
}
