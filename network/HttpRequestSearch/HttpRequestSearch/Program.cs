using System.Net;
using System.IO;


void GetHeader(string data)
{
  int h1Index = data.IndexOf("h1");
  // find bracket
  while (data[h1Index] != '>')
  {
    h1Index++;
  }
  h1Index++;
  // outpu header
  Console.WriteLine("Header:");
  while (data[h1Index] != '<')
  {
    Console.Write(data[h1Index]);
    h1Index++;
  }
  Console.WriteLine();
}

void GetTownLinks(string data)
{
  int markerCityIndex = data.IndexOf("marker-city");
  while (markerCityIndex != -1)
  {
    // find start of name
    for(int i = 0; i < 2; i++)
    {
      while (data[markerCityIndex] != '>')
      {
        markerCityIndex++;
      }
      markerCityIndex++;
    }
    while (data[markerCityIndex] != '<')
    {
      Console.Write(data[markerCityIndex]);
      markerCityIndex++;
    }
    Console.Write(": ");
    while (data[markerCityIndex] != '\"')
    {
      markerCityIndex++;
    }
    markerCityIndex++;
    while (data[markerCityIndex] != '\"')
    {
      Console.Write(data[markerCityIndex]);
      markerCityIndex++;
    }
    Console.WriteLine();

    data = data.Substring(markerCityIndex);
    markerCityIndex = data.IndexOf("marker-city");
  }
}

Console.OutputEncoding = System.Text.Encoding.UTF8;
HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://itstep.org");
request.Method = "GET";
request.AllowAutoRedirect = true;
HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

using (StreamReader reader = new StreamReader(response.GetResponseStream()))
{
  string data = reader.ReadToEnd(); // html page
  GetHeader(data);
  GetTownLinks(data);

  
  //Console.WriteLine(data);
  
}

/*
WebHeaderCollection headers = response.Headers;
for (int i = 0; i < headers.Count; i++)
{
  Console.WriteLine("{0}: {1}", headers.GetKey(i), headers[i]);
}
response.Close();
*/


/*
using (Stream stream = response.GetResponseStream())
{
  using (StreamReader reader = new StreamReader(stream))
  {
    Console.WriteLine(reader.ReadToEnd());
  }
}
*/


response.Close();