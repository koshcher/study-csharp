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

namespace SignUpBinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // возможно это стоило и в файл занести
        List<string> countryList = new List<string> { "Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Antigua and Barbuda",
            "Argentina", "Armenia", "Australia", "Austria", "Azerbaijan", "The Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus",
            "Belgium", "Belize", "Benin", "Bhutan", "Bolivia", "Bosnia and Herzegovina", "Botswana", "Brazil", "Brunei", "Bulgaria",
            "Burkina Faso", "Burundi", "Cabo Verde", "Cambodia", "Cameroon", "Canada", "Central African Republic", "Chad", "Chile",
            "China", "Colombia", "Comoros", "Congo", "Congo, Republic of the", "Costa Rica", "Côte d’Ivoire",
            "Croatia", "Cuba", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "East Timor",
            "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia", "Eswatini", "Ethiopia", "Fiji", "Finland",
            "France", "Gabon", "The Gambia", "Georgia", "Germany", "Ghana", "Greece", "Grenada", "Guatemala", "Guinea", "Guinea-Bissau",
            "Guyana", "Haiti", "Honduras", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland", "Israel", "Italy", "Jamaica",
            "Japan", "Jordan", "Kazakhstan", "Kenya", "Kiribati", "Korea, North", "Korea, South", "Kosovo", "Kuwait", "Kyrgyzstan", "Laos",
            "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein", "Lithuania", "Luxembourg", "Madagascar", "Malawi", "Malaysia",
            "Maldives", "Mali", "Malta", "Marshall Islands", "Mauritania", "Mauritius", "Mexico", "Micronesia, Federated States of", "Moldova",
            "Monaco", "Mongolia", "Montenegro", "Morocco", "Mozambique", "Myanmar (Burma)", "Namibia", "Nauru", "Nepal", "Netherlands",
            "New Zealand", "Nicaragua", "Niger", "Nigeria", "North Macedonia", "Norway", "Oman", "Pakistan", "Palau", "Panama", "Papua New Guinea",
            "Paraguay", "Peru", "Philippines", "Poland", "Portugal", "Qatar", "Romania", "Russia", "Rwanda", "Saint Kitts and Nevis",
            "Saint Lucia", "Saint Vincent and the Grenadines", "Samoa", "San Marino", "Sao Tome and Principe", "Saudi Arabia", "Senegal",
            "Serbia", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "Solomon Islands", "Somalia", "South Africa", "Spain",
            "Sri Lanka", "Sudan", "Sudan, South", "Suriname", "Sweden", "Switzerland", "Syria", "Taiwan", "Tajikistan", "Tanzania", "Thailand",
            "Togo", "Tonga", "Trinidad and Tobago", "Tunisia", "Turkey", "Turkmenistan", "Tuvalu", "Uganda", "Ukraine", "United Arab Emirates",
            "United Kingdom", "United States", "Uruguay", "Uzbekistan", "Vanuatu", "Vatican City", "Venezuela", "Vietnam", "Yemen", "Zambia",
            "Zimbabwe" };

        Dictionary<string, int> months = new Dictionary<string, int> { { "January", 31 }, { "February", 28 }, { "March", 31 }, { "April", 30 },
            { "May", 31 }, { "June", 30 }, { "July", 31 }, { "August", 31 }, { "September", 30 }, { "October", 31 }, { "November", 30 }, { "December", 31 } };

        Dictionary<TextBox, string> enters;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new User();
            enters = new Dictionary<TextBox, string>
            {
                {firstNameTextBox, "First Name"},
                {lastNameTextBox, "Last Name"},
                {screenNameTextBox, "Screen Name"},
                {emailTextBox, "E-mail"},
                {phoneTextBox, "Phone"}
            };
            foreach (var country in countryList)
            {
                countryComboBox.Items.Add(country);
            }
            countryComboBox.SelectedIndex = 0;

            foreach (var month in months)
            {
                monthComboBox.Items.Add(month.Key);
            }
            monthComboBox.SelectedIndex = 0;
            UpdateDays();

            for (int i = DateTime.Now.Year; i >= 1900; i--)
            {
                yearComboBox.Items.Add(i.ToString());
            }
            yearComboBox.SelectedIndex = 0;

            ((User)DataContext).BirthDate = new Date { Day = Convert.ToInt32(dayComboBox.SelectedItem), Month = monthComboBox.SelectedItem.ToString(), Year = Convert.ToInt32(yearComboBox.SelectedItem) };
        }

        private void UpdateDays()
        {
            int daysCount;
            if (months.TryGetValue(monthComboBox.SelectedItem.ToString(), out daysCount))
            {
                dayComboBox.Items.Clear();
                for (int i = 1; i <= daysCount; i++)
                {
                    dayComboBox.Items.Add(i.ToString());
                }
                dayComboBox.SelectedIndex = 0;
            }
        }

        private void monthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDays();
        }

        private void yearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DateTime.IsLeapYear(Convert.ToInt32(yearComboBox.SelectedItem)))
            {
                months["February"] = 29;
            }
            else
            {
                months["February"] = 28;
            }
            UpdateDays();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            ((User)DataContext).FirstName = firstNameTextBox.Text;
            ((User)DataContext).LastName = lastNameTextBox.Text;
            ((User)DataContext).ScreenName = screenNameTextBox.Text;

            ((User)DataContext).BirthDate = new Date { Day = Convert.ToInt32(dayComboBox.SelectedItem), Month = monthComboBox.SelectedItem.ToString(), Year = Convert.ToInt32(yearComboBox.SelectedItem)};

            if (maleRadioButton.IsChecked == true)
            {
                ((User)DataContext).Gender = "Male";
            } 
            else
            {
                ((User)DataContext).Gender = "Female";
            }
            ((User)DataContext).Country = countryComboBox.SelectedItem.ToString();

            ((User)DataContext).Email = emailTextBox.Text;
            ((User)DataContext).Phone = phoneTextBox.Text;
            ((User)DataContext).Password = passwordTextBox.Password;
            ((User)DataContext).ConfirmPassword = confirmPasswordTextBox.Password;

            if(isAgreeCheckBox.IsChecked == true)
            {
                ((User)DataContext).IsAgreeWithTerms = "Agree";
            }
            else
            {
                ((User)DataContext).IsAgreeWithTerms = "Disagree";
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                if (((TextBox)sender).Foreground == Brushes.Gray)
                {
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Foreground = Brushes.Black;
                }
            }
        }

        private void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                if (((TextBox)sender).Text.Trim().Equals(""))
                {
                    ((TextBox)sender).Foreground = Brushes.Gray;
                    string ask = "";
                    if(enters.TryGetValue((TextBox)sender, out ask))
                    {
                        ((TextBox)sender).Text = "Enter " + ask + "...";
                    } 
                    else
                    {
                        ((TextBox)sender).Text = "Enter";
                    }
                }
            }
        }
    }
}
