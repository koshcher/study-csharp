using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BookFinder
{
  public class Book
  {
    public string Title { get; set; }
    public string PageLink { get; set; }
    public string Image { get; set; } = "";

    public string? Text
    {
      get {
        string? textLink = GetTextLink();
        if(textLink != null)
        {
          HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("https://www.gutenberg.org{0}", textLink));
          HttpWebResponse response = (HttpWebResponse)request.GetResponse();
          using (StreamReader reader = new StreamReader(response.GetResponseStream()))
          {
            return reader.ReadToEnd();
          }
        }
        return null;
      }
    }

    private string? GetTextLink()
    {
      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("https://www.gutenberg.org{0}", PageLink));
      HttpWebResponse response = (HttpWebResponse)request.GetResponse();
      using (StreamReader reader = new StreamReader(response.GetResponseStream()))
      {
        string data = reader.ReadToEnd();
        int textlinkIndex = data.IndexOf("title=\"Download\">Plain Text UTF-8");
        if (textlinkIndex > -1)
        {
          data = data.Substring(0, textlinkIndex);
          int hrefIndex = data.LastIndexOf("href=\"") + 6;
          string link = "";
          while (data[hrefIndex] != '\"')
          {
            link += data[hrefIndex];
            hrefIndex++;
          }
          return link;
        }
        return null;
      }
    }
  }
}

