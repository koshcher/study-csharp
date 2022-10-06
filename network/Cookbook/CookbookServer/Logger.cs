using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CookbookServer
{
  internal class Logger
  {
    private string fileName = "log.txt";
    public void LogConnection(IPEndPoint client)
    {
      using (StreamWriter streamWriter = new StreamWriter(fileName, true))
      {
        streamWriter.WriteLine(string.Format("{0}:{1} connected at {2}", 
          client.Address.ToString(), client.Port.ToString(), DateTime.Now.ToString()));
      }
    }

    public void LogDisconnect(IPEndPoint client)
    {
      using (StreamWriter streamWriter = new StreamWriter(fileName, true))
      {
        streamWriter.WriteLine(string.Format("{0}:{1} disconnected at {2}",
          client.Address.ToString(), client.Port.ToString(), DateTime.Now.ToString()));
      }
    }
    public void LogRequest(string message, IPEndPoint client)
    {
      using (StreamWriter streamWriter = new StreamWriter(fileName, true))
      {
        streamWriter.WriteLine(string.Format("{0}:{1} requested: '{2}' at {3}",
          client.Address.ToString(), client.Port.ToString(), message, DateTime.Now.ToString()));
      }
    }

  }
}
