using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace SignUpBinding
{
    internal sealed class User : INotifyPropertyChanged
    {
        string firstName = "First Name";
        string lastName = "Last Name";
        string screenName = "Screen Name";
        Date birthDate = new Date();
        string gender = "Gender";
        string country = "Country";
        string email = "E-mail";
        string phone = "Phone";
        string password = "Password";
        string confirmPassword = "Confirm Password";
        string isAgreeWithTerms = "Disagree";

        public string FirstName
        {
            get => firstName;
            set { SetValue(ref firstName, value, nameof(FirstName)); }
        }
        public string LastName
        {
            get => lastName;
            set { SetValue(ref lastName, value, nameof(LastName)); }
        }
        public string ScreenName
        {
            get => screenName;
            set { SetValue(ref screenName, value, nameof(ScreenName)); }
        }
        public Date BirthDate
        {
            get => birthDate;
            set {
                if (!this.birthDate.Equals(value))
                {
                    this.birthDate = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(BirthDate)));
                }
            }
        }
        public string Gender
        {
            get => gender;
            set { SetValue(ref gender, value, nameof(Gender)); }
        }
        public string Country
        {
            get => country;
            set { SetValue(ref country, value, nameof(Country)); }
        }
        public string Email
        {
            get => email;
            set { SetValue(ref email, value, nameof(Email)); }
        }
        public string Phone
        {
            get => phone;
            set { SetValue(ref phone, value, nameof(Phone)); }
        }
        public string Password
        {
            get => password;
            set { SetValue(ref password, value, nameof(Password)); }
        }
        public string ConfirmPassword
        {
            get => confirmPassword;
            set { SetValue(ref confirmPassword, value, nameof(ConfirmPassword)); }
        }
        public string IsAgreeWithTerms
        {
            get => isAgreeWithTerms;
            set { SetValue(ref isAgreeWithTerms, value, nameof(IsAgreeWithTerms)); }
        }

        private void SetValue(ref string sourth, string value, string property)
        {
            if (!sourth.Equals(value))
            {
                sourth = value;
                OnPropertyChanged(new PropertyChangedEventArgs(property));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}
