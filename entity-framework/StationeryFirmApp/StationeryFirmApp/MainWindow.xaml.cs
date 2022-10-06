using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace StationeryFirmApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlConnection conn;
        private SqlDataAdapter dataAdapter;
        private DataTable dataTable;
        private SqlCommandBuilder cmdBuilder;

        public MainWindow()
        {
            InitializeComponent();
            showComboBox.SelectedIndex = 0;
            showSpecialComboBox.SelectedIndex = 0;
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AzureStationeryFirmConnString"].ConnectionString);
            AsyncRadioButton.IsChecked = true;
        }

        private void ExecuteRequest(string request)
        {
            if(AsyncRadioButton.IsChecked == true)
            {
                AsyncExecuteRequest(request);
            }
            else
            {
                SyncExecuteRequest(request);
            }
        }

        /* Without Task
        private async void AsyncExecuteRequest(string request)
        {
            try
            {
                DateTime start = DateTime.Now;
                showBtn.IsEnabled = false;

                await conn.OpenAsync();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText =  request;

                dataTable = new DataTable();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    int line = 0;
                    do
                    {
                        while (await reader.ReadAsync())
                        {
                            if (line == 0)
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    dataTable.Columns.Add(reader.GetName(i));
                                }
                                line++;
                            }
                            DataRow row = dataTable.NewRow();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[i] = await reader.GetFieldValueAsync<Object>(i);
                            }
                            dataTable.Rows.Add(row);
                        }
                    } while (reader.NextResult());
                }

                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = dataTable.DefaultView;
                conn.Close();
                showBtn.IsEnabled = true;
                timeTextBlock.Text = "Кол-во секунд для выполнения запроса: " + (DateTime.Now - start).Seconds.ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Преблема при выполнении запроса");
                showBtn.IsEnabled = true;
            }
        }
        */

        private async void AsyncExecuteRequest(string request)
        {
            try
            {
                DateTime start = DateTime.Now;
                showBtn.IsEnabled = false;
                dataGrid.ItemsSource = null;
                dataTable = new DataTable();

                await Task.Run(() =>
                {
                    dataAdapter = new SqlDataAdapter(request, conn);
                    cmdBuilder = new SqlCommandBuilder(dataAdapter);
                    dataAdapter.Fill(dataTable);
                });

                dataGrid.ItemsSource = dataTable.DefaultView;
                showBtn.IsEnabled = true;
                timeTextBlock.Text = "Кол-во секунд для выполнения запроса: " + (DateTime.Now - start).Seconds.ToString();
            }
            catch (Exception ex)
            {
                showBtn.IsEnabled = true;
                MessageBox.Show("Преблема при выполнении запроса");
            }
        }

        private void SyncExecuteRequest(string request)
        {
            try
            {
                DateTime start = DateTime.Now;
                dataTable = new DataTable();
                dataAdapter = new SqlDataAdapter(request, conn);
                dataGrid.ItemsSource = null;
                cmdBuilder = new SqlCommandBuilder(dataAdapter);
                dataAdapter.Fill(dataTable);

                dataGrid.ItemsSource = dataTable.DefaultView;
                timeTextBlock.Text = "Кол-во секунд для выполнения запроса: " + (DateTime.Now - start).Seconds.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Преблема при выполнении запроса");
            }
        }

        private void Show(object sender, RoutedEventArgs e)
        {
            int num;
            switch (showComboBox.SelectedIndex)
            {
                case 0:
                    ExecuteRequest("select * from Stationery");
                    break;

                case 1:
                    ExecuteRequest("select * from StationeryTypes");
                    break;

                case 2:
                    ExecuteRequest("select * from Managers");
                    break;

                case 3:
                    ExecuteRequest("select * from BuyerFirms");
                    break;

                case 4:
                    ExecuteRequest("SELECT TOP 1 Name, Count FROM Stationery ORDER BY Count DESC");
                    break;

                case 5:
                    ExecuteRequest("SELECT TOP 1 Name, Count FROM Stationery ORDER BY Count ASC");
                    break;

                case 6:
                    ExecuteRequest("SELECT TOP 1 Name, CostPrice FROM Stationery ORDER BY CostPrice ASC");
                    break;

                case 7:
                    ExecuteRequest("SELECT TOP 1 Name, CostPrice FROM Stationery ORDER BY CostPrice DESC");
                    break;

                case 8:
                    if (int.TryParse(showTextBox.Text, out num))
                    {
                        ExecuteRequest("SELECT * FROM Stationery WHERE Type=" + num);
                    }
                    else
                    {
                        MessageBox.Show("Введены некорретные данные");
                    }
                    break;

                case 9:
                    if (int.TryParse(showTextBox.Text, out num))
                    {
                        ExecuteRequest(@"SELECT Stationery.*
                            FROM Sales
                            JOIN Stationery
                            ON Stationery.Id=Sales.Stationery
                            WHERE Sales.Manager=" + num.ToString());
                    }
                    else
                    {
                        MessageBox.Show("Введены некорретные данные");
                    }
                    break;

                case 10:
                    if (int.TryParse(showTextBox.Text, out num))
                    {
                        ExecuteRequest(@"SELECT Stationery.Name
                            FROM Sales
                            JOIN Stationery
                            ON Stationery.Id=Sales.Stationery
                            WHERE Sales.BuyerFirm=" + num.ToString());
                    }
                    else
                    {
                        MessageBox.Show("Введены некорретные данные");
                    }
                    break;

                case 11:
                    ExecuteRequest("SELECT TOP 1 * FROM Sales ORDER BY Date DESC");
                    break;

                case 12:
                    ExecuteRequest(@"SELECT StationeryTypes.Name, AVG(Stationery.Count) as 'Среднее кол-во'
                            FROM Stationery
                            JOIN StationeryTypes
                            ON StationeryTypes.Id=Stationery.Type
                            GROUP BY StationeryTypes.Name");
                    break;
            }
        }

        private void showComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (showComboBox.SelectedIndex > 7 && showComboBox.SelectedIndex < 11)
            {
                showTextBox.IsEnabled = true;
                MessageBox.Show("Введите индекс предмета поиска в TextBox возле меню");
            }
            else
            {
                showTextBox.IsEnabled = false;
            }
        }

        private void ApplyChanges(object sender, RoutedEventArgs e)
        {
            if (AsyncRadioButton.IsChecked == true)
            {
                AsyncApplyChanges();
            }
            else
            {
                SyncApplyChanges();
            }
        }

        private async void AsyncApplyChanges()
        {
            if (dataTable != null)
            {
                await Task.Run(() =>
                {
                    try
                    {
                        dataAdapter.Update(dataTable);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Данный запрос не поддерживает изменения");
                    }
                });
            }
        }

        private void SyncApplyChanges()
        {
            try
            {
                if (dataTable != null)
                {
                    dataAdapter.Update(dataTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Данный запрос не поддерживает изменения");
            }
        }

        private string BetweenDates()
        {
            string firstDate = firstDatePicker.DisplayDate.Month.ToString() + "/" + firstDatePicker.DisplayDate.Day.ToString() + "/" + firstDatePicker.DisplayDate.Year.ToString();
            string secondDate = secondDatePicker.DisplayDate.Month.ToString() + "/" + secondDatePicker.DisplayDate.Day.ToString() + "/" + secondDatePicker.DisplayDate.Year.ToString();
            return "'" + firstDate + "' AND '" + secondDate + "'";
        }

        private void ShowSpecial(object sender, RoutedEventArgs e)
        {
            switch (showSpecialComboBox.SelectedIndex)
            {
                case 0:
                    ExecuteRequest(@"SELECT TOP 1 Managers.Name, SUM(Sales.Count)
                            FROM Sales
                            JOIN Managers
                            ON Sales.Manager=Managers.Id
                            GROUP BY Managers.Name
                            ORDER BY SUM(Sales.Count) DESC");
                    break;

                case 1:
                    ExecuteRequest(@"SELECT TOP 1 Managers.Name, SUM(Sales.Count * (Stationery.CostPrice - Stationery.UnitPrice))
                            FROM Sales, Managers, Stationery
                            WHERE Sales.Manager=Managers.Id AND Sales.Stationery=Stationery.Id
                            GROUP BY Managers.Name
                            ORDER BY SUM(Sales.Count * (Stationery.CostPrice - Stationery.UnitPrice)) DESC");
                    break;

                case 2:
                    ExecuteRequest(@"SELECT TOP 1 Managers.Name, SUM(Sales.Count * (Stationery.CostPrice - Stationery.UnitPrice))
                            FROM Sales, Managers, Stationery
                            WHERE Sales.Manager=Managers.Id AND Sales.Stationery=Stationery.Id AND Sales.Date BETWEEN "
                            + BetweenDates() +
                            @" GROUP BY Managers.Name
                            ORDER BY SUM(Sales.Count * (Stationery.CostPrice - Stationery.UnitPrice)) DESC");
                    break;

                case 3:
                    ExecuteRequest(@"SELECT TOP 1 BuyerFirms.Name, SUM(Sales.Count * Stationery.CostPrice)
                            FROM Sales, BuyerFirms, Stationery
                            WHERE Sales.BuyerFirm=BuyerFirms.Id AND Sales.Stationery=Stationery.Id
                            GROUP BY BuyerFirms.Name
                            ORDER BY SUM(Sales.Count * Stationery.CostPrice) DESC");
                    break;

                case 4:
                    ExecuteRequest(@"SELECT TOP 1 StationeryTypes.Name, SUM(Sales.Count)
                            FROM Sales, StationeryTypes, Stationery
                            WHERE Stationery.Type=StationeryTypes.Id AND Sales.Stationery=Stationery.Id
                            GROUP BY StationeryTypes.Name
                            ORDER BY SUM(Sales.Count) DESC");
                    break;

                case 5:
                    ExecuteRequest(@"SELECT TOP 1 StationeryTypes.Name, SUM(Sales.Count * (Stationery.CostPrice - Stationery.UnitPrice))
                            FROM Sales, StationeryTypes, Stationery
                            WHERE Stationery.Type=StationeryTypes.Id AND Sales.Stationery=Stationery.Id
                            GROUP BY StationeryTypes.Name
                            ORDER BY SUM(Sales.Count * (Stationery.CostPrice - Stationery.UnitPrice)) DESC");
                    break;

                case 6:
                    ExecuteRequest(@"SELECT TOP 1 Stationery.Name, SUM(Sales.Count)
                            FROM Sales, Stationery
                            WHERE Sales.Stationery=Stationery.Id
                            GROUP BY Stationery.Name
                            ORDER BY SUM(Sales.Count) DESC");
                    break;

                case 7:
                    int num;
                    if(int.TryParse(showSpecialTextBox.Text, out num))
                    {
                        DateTime date = DateTime.Now;
                        ExecuteRequest(@"SELECT DISTINCT Stationery.Name
                            FROM Sales, Stationery
                            WHERE Sales.Stationery=Stationery.Id AND DATEDIFF(day, Sales.Date, '"
                            + date.Month.ToString() + "/" + date.Day.ToString() + "/" + date.Year.ToString() +
                            "') >= " + num.ToString());
                    }
                    else
                    {
                        MessageBox.Show("Введены некорретные данные");
                    }
                    break;
            }
        }

        private void showSpecialComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(showSpecialComboBox.SelectedIndex == 2)
            {
                firstDatePicker.IsEnabled = true;
                secondDatePicker.IsEnabled = true;
                firstDatePicker.Background = Brushes.White;
                secondDatePicker.Background = Brushes.White;
            }
            else
            {
                firstDatePicker.IsEnabled = false;
                secondDatePicker.IsEnabled = false;
                firstDatePicker.Background = Brushes.LightGray;
                secondDatePicker.Background = Brushes.LightGray;
            }
            if(showSpecialComboBox.SelectedIndex == 7)
            {
                showSpecialTextBox.IsEnabled = true;
                showSpecialTextBox.Background = Brushes.White;
            } 
            else
            {
                showSpecialTextBox.IsEnabled = false;
                showSpecialTextBox.Background = Brushes.LightGray;
            }
        }
    }
}