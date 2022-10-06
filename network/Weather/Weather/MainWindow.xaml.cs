using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace Weather
{
  public partial class MainWindow : Window
  {
    private WeatherService ws = new WeatherService();

    public MainWindow()
    {
      InitializeComponent();
    }

    private void Load_Click(object sender, RoutedEventArgs e)
    {
      string city;
      if (CityTxt.Text != "")
      {
        city = CityTxt.Text;
      }
      else
      {
        MessageBox.Show("Field is empty!");
        return;
      }

      Forecast.Text += ws.GetWeather(city);
      CityTxt.Text = "";
    }
  }
}