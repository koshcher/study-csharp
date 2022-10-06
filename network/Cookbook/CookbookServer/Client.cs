using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CookbookServer
{
  internal class Client
  {
    IPEndPoint iPEndPoint { get; set; }
    DateTime connectTime { get; set; }
    Client(IPEndPoint iPEndPoint, DateTime connectTime)
    {
      this.iPEndPoint = iPEndPoint;
      this.connectTime = connectTime;
    }
  }
}
