using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace VegetableWarehouseApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection conn = null;
        SqlCommand cmd;
        DataTable table;
        SqlDataReader reader;

        public MainWindow()
        {
            InitializeComponent();
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLemonConnString"].ConnectionString);
        }

        void ExecuteRequest(string request)
        {
            try
            {
                conn.Open();

                cmd = new SqlCommand(request, conn);
                dataGrid.ItemsSource = null;

                table = new DataTable();
                reader = cmd.ExecuteReader();

                bool isFirstLine = true;
                do
                {
                    while(reader.Read())
                    {
                        if(isFirstLine)
                        {
                            for(int i = 0; i < reader.FieldCount; i++)
                            {
                                table.Columns.Add(reader.GetName(i));
                            }
                            isFirstLine = false;
                        }
                        DataRow row = table.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[i] = reader[i];
                        }
                        table.Rows.Add(row);
                    }
                } while (reader.NextResult());

                dataGrid.ItemsSource = table.DefaultView;
            }
            catch
            {
                MessageBox.Show("There is problem with input");
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        private void AllInfo(object sender, RoutedEventArgs e)
        {
            ExecuteRequest("select * from Food");
        }

        private void AllNames(object sender, RoutedEventArgs e)
        {
            ExecuteRequest("select Name from Food");
        }
        private void AllColors(object sender, RoutedEventArgs e)
        {
            ExecuteRequest("select DISTINCT Color from Food");
        }
        private void MaxCalories(object sender, RoutedEventArgs e)
        {
            ExecuteRequest("select MAX(Calories) from Food");
        }
        private void MinCalories(object sender, RoutedEventArgs e)
        {
            ExecuteRequest("select MIN(Calories) from Food");
        }
        private void AverageCalories(object sender, RoutedEventArgs e)
        {
            ExecuteRequest("select AVG(Calories) from Food");
        }
        private void VegetableCount(object sender, RoutedEventArgs e)
        {
            ExecuteRequest(@"select Count(Id)
                             from Food
                            where Type='овощ'");
        }
        private void FruitCount(object sender, RoutedEventArgs e)
        {
            ExecuteRequest(@"select Count(Id)
                             from Food
                            where Type='фрукт'");
        }
        private void CountInputColor(object sender, RoutedEventArgs e)
        {
            ExecuteRequest(@"select COUNT(Id)
                             from Food
                            where Color='" + input.Text.ToLower() + "'");
        }
        private void CountEachColor(object sender, RoutedEventArgs e)
        {
            ExecuteRequest(@"select COUNT(Color), Color
                             from Food
                            GROUP BY Color");
        }
        private void LessCalories(object sender, RoutedEventArgs e)
        {
            ExecuteRequest(@"select *
                             from Food
                            where Calories < " + input.Text);
        }
        private void MoreCalories(object sender, RoutedEventArgs e)
        {
            ExecuteRequest(@"select *
                             from Food
                            where Calories > " + input.Text);
        }
        private void RangeCalories(object sender, RoutedEventArgs e)
        {
            List<string> range = new List<string>(input.Text.Split(' '));
            if(range.Count < 2)
            {
                MessageBox.Show("Диапозон указан неверно");
            } else
            {
                ExecuteRequest(@"select *
                                from Food
                                where Calories BETWEEN " + range[0] + " AND " + range[1]);
            }   
        }
        private void YellowOrRedFood(object sender, RoutedEventArgs e)
        {
            ExecuteRequest(@"select *
                             from Food
                            where Color='жёлтый' OR Color='красный'");
        }
    }
}
