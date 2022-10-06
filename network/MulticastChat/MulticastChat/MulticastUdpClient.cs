using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MulticastChat
{
    public class MulticastUdpClient
    {
        UdpClient _udpclient;
        int _port;
        IPAddress _multicastIPaddress;
        IPAddress _localIPaddress;
        IPEndPoint _localEndPoint;
        IPEndPoint _remoteEndPoint;

        public MulticastUdpClient(IPAddress multicastIPaddress, int port, IPAddress localIPaddress = null)
        {
            _multicastIPaddress = multicastIPaddress;
            _port = port;
            _localIPaddress = localIPaddress;
            if (localIPaddress == null)
                _localIPaddress = IPAddress.Any;

            _remoteEndPoint = new IPEndPoint(_multicastIPaddress, port);
            _localEndPoint = new IPEndPoint(_localIPaddress, port);

            _udpclient = new UdpClient();

            _udpclient.ExclusiveAddressUse = false;
            _udpclient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _udpclient.ExclusiveAddressUse = false;
           
            _udpclient.Client.Bind(_localEndPoint);
            _udpclient.JoinMulticastGroup(_multicastIPaddress, _localIPaddress);

           
            _udpclient.BeginReceive(new AsyncCallback(ReceivedCallback), null);
        }

      
        public void SendMulticast(byte[] bufferToSend)
        {
            _udpclient.Send(bufferToSend, bufferToSend.Length, _remoteEndPoint);
        }

     
        private void ReceivedCallback(IAsyncResult ar)
        {
         
            IPEndPoint sender = new IPEndPoint(0, 0);
            Byte[] receivedBytes = _udpclient.EndReceive(ar, ref sender);

      
            if (UdpMessageReceived != null)
                UdpMessageReceived(this, new UdpMessageReceivedEventArgs() { Buffer = receivedBytes });

        
            _udpclient.BeginReceive(new AsyncCallback(ReceivedCallback), null);
        }

    
        public event EventHandler<UdpMessageReceivedEventArgs> UdpMessageReceived;

        public class UdpMessageReceivedEventArgs : EventArgs
        {
            public byte[] Buffer { get; set; }
        }

    }
}
