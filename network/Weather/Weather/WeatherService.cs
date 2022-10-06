using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Weather
{
  internal struct Temperature
  {
    public float temp { get; set; }
  }

  internal struct WeatherForecast
  {
    public Temperature main { get; set; }
    public string name { get; set; }
  }

  public class WeatherService
  {
    private string appid = "7e4841e967b5dc658773244a4083a818";

    public string GetWeather(string city)
    {
      string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&units=metric&appid={1}", city, appid);
      HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
      HttpWebResponse response;

      response = (HttpWebResponse)request.GetResponse();

      string resp;
      using (StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.Default))
      {
        resp = stream.ReadToEnd();
      }
      WeatherForecast weather = JsonConvert.DeserializeObject<WeatherForecast>(resp);

      return string.Format("Temperature in {0} is {1} \u00B0C\n", weather.name, weather.main.temp);
    }
  }
}