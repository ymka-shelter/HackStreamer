using System;
using WindowsMediaLib;
using System.Net.Sockets;
using System.Net;

namespace Hack
{
    // Custom implementation to send a stream over network
    class WriterNetworkSink : IWMWriterSink
    {
        Sender s = null;

        // Works as a streamer to a remote server
        public WriterNetworkSink(string url)
        {
            s = new Sender(url);
        }
        // Works as a TCP listener on a given port
        public WriterNetworkSink(int port)
        {
            s = new Sender(port);
        }
        void IWMWriterSink.AllocateDataUnit(int cbDataUnit, out INSSBuffer ppDataUnit)
        {
            ppDataUnit = new ManBuffer(cbDataUnit);
        }

        void IWMWriterSink.IsRealTime(out bool pfRealTime)
        {
            pfRealTime = true;
        }
        void IWMWriterSink.OnDataUnit(INSSBuffer pDataUnit)
        {
            ManBuffer m = (ManBuffer)pDataUnit;
            s.Send(m.Buffer, 0, m.UsedLength);
        }

        void IWMWriterSink.OnEndWriting()
        {
            s.Close();
        }

        void IWMWriterSink.OnHeader(INSSBuffer pHeader)
        {
            ManBuffer m = (ManBuffer)pHeader;
            s.Send(m.Buffer, 0, m.UsedLength);
        }
    }
    class Sender
    {
        private Socket s = null;
        private Socket listener = null;
        public Sender (int port)
        {
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Parse("0.0.0.0"), port));
            listener.Listen(1);
            listener.BeginAccept(onConnection, listener);
            Console.WriteLine("[LISTENING] for connections on port: {0}", port);
        }
        public Sender (string url)
        {
            url = url.ToLower();
            Uri uri = null;
            
            if (url.StartsWith("udp://"))
            {
               uri = new Uri(url);
               s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            } else
            {
               if (!url.StartsWith("tcp://"))
                   url = "tcp://" + url;
               uri = new Uri(url);
               s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }

            s.Connect(uri.Host, uri.Port);
            Console.WriteLine("[CONNECTION] to {0} {1} {2} is established", uri.Scheme, uri.Host, uri.Port);
        }
        private void onConnection (IAsyncResult ar)
        {
            Socket temp = (Socket)ar.AsyncState;
            s = temp.EndAccept(ar);
        }
        public void Send (byte[] data, int offset, int howMany)
        {
            if (s != null) {
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.SetBuffer(data, offset, howMany);
                s.SendAsync(args);
            }
        }
        public void Close ()
        {
            if (s != null)
                s.Close();
            if (listener != null)
                listener.Close();
        }
    }
}
