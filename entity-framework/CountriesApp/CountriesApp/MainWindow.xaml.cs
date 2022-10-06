using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CountriesApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataClasses1DataContext db;

        public MainWindow()
        {
            InitializeComponent();
            db = new DataClasses1DataContext();
            Task2ComboBox.SelectedIndex = 0;
            Task3ComboBox.SelectedIndex = 0;
            Task4ComboBox.SelectedIndex = 0;
        }

        private void Task2Btn_Click(object sender, RoutedEventArgs e)
        {
            switch (Task2ComboBox.SelectedIndex)
            {
                case 0:
                    ShowDataGrid.ItemsSource = from country in db.Countries
                                               from region in db.Regions
                                               from city in db.MajorCities
                                               where region.Id == country.Region
                                               where city.Capital == true
                                               where city.Country == country.Id
                                               select new
                                               {
                                                   Id = country.Id,
                                                   Name = country.Name,
                                                   Square = country.Square,
                                                   CountCitizens = country.CountCitizens,
                                                   Region = region.Name,
                                                   Capital = city.Name
                                               };
                    break;

                case 1:
                    ShowDataGrid.ItemsSource = from country in db.Countries
                                               select new { Name = country.Name };
                    break;

                case 2:
                    ShowDataGrid.ItemsSource = from city in db.MajorCities
                                               where city.Capital == true
                                               select new { Name = city.Name };
                    break;

                case 3:
                    ShowDataGrid.ItemsSource = from city in db.MajorCities
                                               where city.Country == Task2CountryComboBox.SelectedIndex + 1
                                               select new { Name = city.Name };
                    break;

                case 4:
                    ShowDataGrid.ItemsSource = from city in db.MajorCities
                                               where city.Capital == true
                                               where city.CountCitizens > 5000000
                                               select new
                                               {
                                                   Name = city.Name,
                                                   CountCitizens = city.CountCitizens
                                               };
                    break;

                case 5:
                    ShowDataGrid.ItemsSource = from country in db.Countries
                                               from region in db.Regions
                                               where region.Name == "европа"
                                               where country.Region == region.Id
                                               select new
                                               {
                                                   Name = country.Name,
                                                   Region = region.Name
                                               };
                    break;

                case 6:
                    double num = 0;
                    if (double.TryParse(Task2TextBox.Text, out num))
                    {
                        ShowDataGrid.ItemsSource = from country in db.Countries
                                                   where country.Square > num
                                                   select new { Name = country.Name, Square = country.Square };
                    }
                    else
                    {
                        MessageBox.Show("Некорректный ввод данных");
                    }
                    break;
            }
        }

        private void Task3Btn_Click(object sender, RoutedEventArgs e)
        {
            switch (Task3ComboBox.SelectedIndex)
            {
                case 0:
                    ShowDataGrid.ItemsSource = from city in db.MajorCities
                                               where city.Capital == true && city.Name.Contains("а") && city.Name.Contains("р")
                                               select new { Name = city.Name, Capital = city.Capital };
                    break;

                case 1:
                    ShowDataGrid.ItemsSource = from city in db.MajorCities
                                               where city.Capital == true && city.Name.StartsWith("к")
                                               select new { Name = city.Name, Capital = city.Capital };
                    break;

                case 2:
                    double num1 = 0;
                    double num2 = 0;
                    if (double.TryParse(Task3TextBox1.Text, out num1) && double.TryParse(Task3TextBox2.Text, out num2))
                    {
                        ShowDataGrid.ItemsSource = from country in db.Countries
                                                   where country.Square <= (num1 > num2 ? num1 : num2) && country.Square >= (num1 < num2 ? num1 : num2)
                                                   select new { Name = country.Name, Square = country.Square };
                    }
                    else
                    {
                        MessageBox.Show("Некорректный ввод данных");
                    }
                    break;

                case 3:
                    double num = 0;
                    if (double.TryParse(Task3TextBox1.Text, out num))
                    {
                        ShowDataGrid.ItemsSource = from country in db.Countries
                                                   where country.CountCitizens > num
                                                   select new { Name = country.Name, CountCitizens = country.CountCitizens };
                    }
                    else
                    {
                        MessageBox.Show("Некорректный ввод данных");
                    }
                    break;
            }
        }

        private void Task4Btn_Click(object sender, RoutedEventArgs e)
        {
            switch (Task4ComboBox.SelectedIndex)
            {
                case 0:
                    ShowDataGrid.ItemsSource = (from country in db.Countries
                                                orderby country.Square descending
                                                select new { Name = country.Name, Square = country.Square }).Take(5);
                    break;

                case 1:
                    ShowDataGrid.ItemsSource = (from city in db.MajorCities
                                                where city.Capital == true
                                                orderby city.CountCitizens descending
                                                select new { Name = city.Name, CountCitizens = city.CountCitizens }).Take(5);
                    break;

                case 2:
                    ShowDataGrid.ItemsSource = (from country in db.Countries
                                                orderby country.Square descending
                                                select new { Name = country.Name, Square = country.Square }).Take(1);
                    break;

                case 3:
                    ShowDataGrid.ItemsSource = (from city in db.MajorCities
                                                where city.Capital == true
                                                orderby city.CountCitizens descending
                                                select new { Name = city.Name, CountCitizens = city.CountCitizens }).Take(5);
                    break;

                case 4:
                    ShowDataGrid.ItemsSource = (from country in db.Countries
                                                join region in db.Regions on country.Region equals region.Id
                                                where region.Name == "европа"
                                                orderby country.Square ascending
                                                select new { Name = country.Name, Square = country.Square, Region = region.Name }).Take(1);
                    break;

                case 5:

                    DataTable dataTableAverage = new DataTable();
                    dataTableAverage.Columns.Add(new DataColumn("Average"));
                    dataTableAverage.Rows.Add((from country in db.Countries
                                               join region in db.Regions on country.Region equals region.Id
                                               where region.Name == "европа"
                                               select country.Square).Average());
                    ShowDataGrid.ItemsSource = dataTableAverage.DefaultView;
                    break;

                case 6:
                    ShowDataGrid.ItemsSource = (from city in db.MajorCities
                                                where city.Country == Task4CountryComboBox.SelectedIndex + 1
                                                orderby city.CountCitizens descending
                                                select new { Name = city.Name, CountCitizens = city.CountCitizens }).Take(3);
                    break;

                case 7:
                    DataTable dataTableCount = new DataTable();
                    dataTableCount.Columns.Add(new DataColumn("Count"));
                    dataTableCount.Rows.Add(db.Countries.Count());
                    ShowDataGrid.ItemsSource = dataTableCount.DefaultView;
                    break;

                case 8:
                    ShowDataGrid.ItemsSource = (from country in db.Countries
                                                group country by country.Region into cg
                                                from region in db.Regions
                                                where region.Id == cg.Key
                                                orderby cg.Count() descending
                                                select new { Name = region.Name, Count = cg.Count() }).Take(1);
                    break;

                case 9:
                    ShowDataGrid.ItemsSource = from country in db.Countries
                                               group country by country.Region into cg
                                               from region in db.Regions
                                               where region.Id == cg.Key
                                               select new { Name = region.Name, Count = cg.Count() };
                    break;
            }
        }

        private void Task2ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (Task2ComboBox.SelectedIndex)
            {
                case 3:
                    Task2CountryComboBox.IsEnabled = true;
                    Task2CountryComboBox.Background = Brushes.White;
                    Task2TextBox.IsEnabled = false;
                    Task2TextBox.Background = Brushes.LightGray;
                    Task2CountryComboBox.ItemsSource = from country in db.Countries
                                                       select country.Name;
                    Task2CountryComboBox.SelectedIndex = 0;
                    break;

                case 6:
                    Task2TextBox.IsEnabled = true;
                    Task2TextBox.Background = Brushes.White;
                    Task2CountryComboBox.IsEnabled = false;
                    Task2CountryComboBox.Background = Brushes.LightGray;
                    break;

                default:
                    Task2CountryComboBox.IsEnabled = false;
                    Task2CountryComboBox.Background = Brushes.LightGray;
                    Task2TextBox.IsEnabled = false;
                    Task2TextBox.Background = Brushes.LightGray;
                    break;
            }
        }

        private void Task3ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (Task3ComboBox.SelectedIndex)
            {
                case 2:
                    Task3TextBox1.IsEnabled = true;
                    Task3TextBox1.Background = Brushes.White;
                    Task3TextBox2.IsEnabled = true;
                    Task3TextBox2.Background = Brushes.White;
                    break;

                case 3:
                    Task3TextBox1.IsEnabled = true;
                    Task3TextBox1.Background = Brushes.White;
                    Task3TextBox2.IsEnabled = false;
                    Task3TextBox2.Background = Brushes.LightGray;
                    break;

                default:
                    Task3TextBox1.IsEnabled = false;
                    Task3TextBox1.Background = Brushes.LightGray;
                    Task3TextBox2.IsEnabled = false;
                    Task3TextBox2.Background = Brushes.LightGray;
                    break;
            }
        }

        private void Task4ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (Task4ComboBox.SelectedIndex)
            {
                case 6:
                    Task4CountryComboBox.IsEnabled = true;
                    Task4CountryComboBox.Background = Brushes.White;
                    Task4CountryComboBox.ItemsSource = from country in db.Countries
                                                       select country.Name;
                    Task4CountryComboBox.SelectedIndex = 0;
                    break;

                default:
                    Task4CountryComboBox.IsEnabled = false;
                    Task4CountryComboBox.Background = Brushes.LightGray;
                    break;
            }
        }
    }
}