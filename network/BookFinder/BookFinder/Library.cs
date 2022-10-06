using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace BookFinder
{
  internal class Library
  {
    List<Book> books = new List<Book>();

    public List<Book> Books { get => books; }

    public void LoadMostPopular()
    {
      books.Clear();

      HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.gutenberg.org/browse/scores/top#books-last1");
      HttpWebResponse response = (HttpWebResponse)request.GetResponse();

      using (StreamReader reader = new StreamReader(response.GetResponseStream()))
      {
        string data = reader.ReadToEnd();
        int startIndex = data.IndexOf("<ol>") + 5; // 4 = <ol> + 1 = \n
        int endIndex = data.IndexOf("</ol>", startIndex) - 1; // -1 = \n
        List<string> lis = new List<string>(data.Substring(startIndex, (endIndex - startIndex)).Split('\n'));

        foreach(string li in lis)
        {
          startIndex = li.IndexOf('\"') + 1;
          endIndex = li.IndexOf('\"', startIndex);
          string link = li.Substring(startIndex, endIndex - startIndex);

          startIndex = endIndex + 2; // ">
          endIndex = li.IndexOf('<', startIndex);
          string title = li.Substring(startIndex, endIndex - startIndex);

          books.Add(new Book() { Title = title, PageLink = link});
        }
      }
    }
    public void LoadSearch(string search)
    {
      books.Clear();

      string requestUrl = string.Format("https://www.gutenberg.org/ebooks/search/?query={0}&submit_search=Go", search.ToLower());
      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
      HttpWebResponse response = (HttpWebResponse)request.GetResponse();

      using (StreamReader reader = new StreamReader(response.GetResponseStream()))
      {
        string data = reader.ReadToEnd(); // html page
        int startIndex = data.IndexOf("<li class=\"booklink\">");
        if(startIndex > -1)
        {
          int endIndex = data.IndexOf("<li class=\"statusline\">", startIndex) - 6; // -1 = \n\  + -5 = </li>
          List<string> lis = new List<string>(data.Substring(startIndex, (endIndex - startIndex)).Split("</li>"));

          foreach (string li in lis)
          {
            startIndex = li.IndexOf("href=\"") + 6;
            endIndex = li.IndexOf('\"', startIndex);
            string link = li.Substring(startIndex, endIndex - startIndex);

            startIndex = li.IndexOf("src=\"") + 5;
            endIndex = li.IndexOf('\"', startIndex);
            string image = string.Format("https://www.gutenberg.org{0}", li.Substring(startIndex, endIndex - startIndex));

            startIndex = li.IndexOf("title\">") + 7;
            endIndex = li.IndexOf('<', startIndex);
            string title = li.Substring(startIndex, endIndex - startIndex);

            books.Add(new Book() { Title = title, PageLink = link, Image = image });
          }
        }
      }
    }
  }
}
