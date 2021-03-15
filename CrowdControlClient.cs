using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TwitchInteraction.CrowdControl
{
    // State object for reading client data asynchronously  
    public sealed class StateObject
    {
        /* Contains the state information. */
        private const int Buffer_Size = 1024;
        private readonly byte[] buffer = new byte[Buffer_Size];
        private readonly Socket listener;
        private readonly int id;
        private StringBuilder sb;

        public StateObject(Socket listener, int id = -1)
        {
            this.listener = listener;
            this.id = id;
            this.Close = false;
            this.Reset();
        }

        public int Id
        {
            get
            {
                return this.id;
            }
        }

        public bool Close { get; set; }

        public int BufferSize
        {
            get
            {
                return Buffer_Size;
            }
        }

        public byte[] Buffer
        {
            get
            {
                return this.buffer;
            }
        }

        public Socket Listener
        {
            get
            {
                return this.listener;
            }
        }

        public string Text
        {
            get
            {
                return this.sb.ToString();
            }
        }

        public void Append(string text)
        {
            this.sb.Append(text);
        }

        public void Reset()
        {
            this.sb = new StringBuilder();
        }
    }

    public delegate void ConnectedHandler(CrowdControlClient c);
    public delegate void ClientMessageReceivedHandler(CrowdControlClient c, string msg);
    public delegate void ClientMessageSubmittedHandler(CrowdControlClient c, bool close);

    public sealed class CrowdControlClient { 
        private const ushort Port = 2679;

        private Socket listener;
        private bool close;

        public event ConnectedHandler Connected;
        public event ClientMessageReceivedHandler MessageReceived;
        public event ClientMessageSubmittedHandler MessageSubmitted;

        public async void StartClient()
        {
            // var host = Dns.GetHostEntry(string.Empty);
            // var ip = host.AddressList[3];
            var ip = IPAddress.Loopback;
            var endpoint = new IPEndPoint(ip, Port);

            while(true) { 
                try
                {
                    this.listener = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    Console.WriteLine("Starting client. IP: " + ip + ", Port: " + Port);
                    await this.ConnectTaskAsync(endpoint);
                
                    var connectedHandler = this.Connected;

                    if (connectedHandler != null)
                    {
                        connectedHandler(this);
                    }
                    return;
                }
                catch (SocketException)
                {
                    Console.WriteLine("Failed to connect to CrowdControl. Retrying in 30 seconds");
                    await Task.Delay(1000 * 30);
                }
            }
        }

        private Task ConnectTaskAsync(IPEndPoint endpoint) 
        {
            return Task.Factory.FromAsync(this.listener.BeginConnect, this.listener.EndConnect, endpoint, null);
        }

        public bool IsConnected()
        {
            return !(this.listener.Poll(1000, SelectMode.SelectRead) && this.listener.Available == 0);
        }

        #region Receive data
        public async void Receive()
        {
            var state = new StateObject(this.listener);

            await this.ReceiveTaskAsync(state);
            state.Listener.BeginReceive(state.Buffer, 0, state.BufferSize, SocketFlags.None, this.ReceiveCallback, state);
            
        }

        private Task ReceiveTaskAsync(StateObject state)
        {
            return Task.Factory.FromAsync(
                       (cb, s) => state.Listener.BeginReceive(state.Buffer, 0, state.BufferSize, SocketFlags.None, cb, s),
                       ias => this.ReceiveCallback(ias),
                       state);
        }

        private async void ReceiveCallback(IAsyncResult result)
        {
            var state = (StateObject)result.AsyncState;
            var receive = state.Listener.EndReceive(result);

            if (receive > 0)
            {
                state.Append(Encoding.UTF8.GetString(state.Buffer, 0, receive));
            }

            // Await null terminated message
            if (receive == state.BufferSize || !state.Text.EndsWith("\0"))
            {
                await this.ReceiveTaskAsync(state);
            }
            else
            {
                var messageReceived = this.MessageReceived;

                if (messageReceived != null)
                {
                    messageReceived(this, state.Text.Replace("\0", ""));
                }

                state.Reset();
            }
        }
        #endregion

        #region Send data
        public async void Send(string msg, bool close)
        {
            if (!this.IsConnected())
            {
                throw new Exception("Destination socket is not connected.");
            }

            // Ensure the response is null terminated
            var response = Encoding.UTF8.GetBytes(msg + "\0");

            this.close = close;
            await this.SendTaskAsync(response);            
        }

        private Task SendTaskAsync(byte[] response)
        {
            return Task.Factory.FromAsync(
                    (sb, s) => this.listener.BeginSend(response, 0, response.Length, SocketFlags.None, this.SendCallback, this.listener),
                    (ias) => this.SendCallback(ias),
                    null
            );            
        }

        private void SendCallback(IAsyncResult result)
        {
            try
            {
                var receiver = (Socket)result.AsyncState;
            }
            catch (SocketException)
            {
                // TODO:
            }
            catch (ObjectDisposedException)
            {
                // TODO;
            }

            var messageSubmitted = this.MessageSubmitted;

            if (messageSubmitted != null)
            {
                messageSubmitted(this, this.close);
            }
        }
        #endregion

        private void Close()
        {
            try
            {
                if (!this.IsConnected())
                {
                    return;
                }

                this.listener.Shutdown(SocketShutdown.Both);
                this.listener.Close();
            }
            catch (SocketException)
            {
                // TODO:
            }
        }

        public void Dispose()
        {
            this.Close();
        }
    }
}
