using System.IO;
using System.Net;

List<string> en;
List<string> ua;

string requestUrl = string.Format("https://raw.githubusercontent.com/roman-koshchei/english-word-lists/main/a-level-words.txt");
HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
HttpWebResponse response = (HttpWebResponse)request.GetResponse();
using (StreamReader reader = new StreamReader(response.GetResponseStream()))
{
  en = new List<string>(reader.ReadToEnd().Split('\n'));
}



requestUrl = string.Format("https://raw-githubusercontent-com.translate.goog/roman-koshchei/english-word-lists/main/a-level-words.txt?_x_tr_sl=en&_x_tr_tl=uk&_x_tr_hl=en&_x_tr_pto=wapp");
request = (HttpWebRequest)WebRequest.Create(requestUrl);
response = (HttpWebResponse)request.GetResponse();
using (StreamReader reader = new StreamReader(response.GetResponseStream()))
{
  string data = reader.ReadToEnd();
  int startIndex = data.IndexOf("<pre>") + 5;
  int endIndex = data.IndexOf("</pre>", startIndex);

  data = data.Substring(startIndex, endIndex - startIndex);

  en = new List<string>(reader.ReadToEnd().Split('\n'));
}

