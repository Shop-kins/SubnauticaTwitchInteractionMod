using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TwitchInteraction
{
    class WebSocketMessageClient : IMessageClient
    {
        public event EventHandler<string> MessageReceived;

        public event EventHandler ConnectionClosed;

        private ClientWebSocket _webSocketClient = new ClientWebSocket();

        private readonly Uri _webSocketServerUri;

        public WebSocketMessageClient(string webSocketServerUrl = "wss://irc-ws.chat.twitch.tv:443")
        {
            _webSocketServerUri = new Uri(webSocketServerUrl);
        }

        public async Task SendMessageAsync(string message, CancellationToken cancellationToken)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            await _webSocketClient.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, cancellationToken);
        }

        public static IEnumerable<T[]> AsBatches<T>(T[] input, int n)
        {
            for (int i = 0, r = input.Length; r >= n; r -= n, i += n)
            {
                var result = new T[n];
                Array.Copy(input, i, result, 0, n);
                yield return result;
            }
        }

        public async Task ConnectAsync(string oauth, string nick, CancellationToken cancellationToken)
        {
            await _webSocketClient.ConnectAsync(_webSocketServerUri, cancellationToken);

            if (_webSocketClient.State == WebSocketState.Open)
            {
                await SendMessageAsync($"PASS {oauth}", cancellationToken);
                await SendMessageAsync($"NICK {nick}", cancellationToken);

                // start receiving messages in separeted thread
                var receive = ReceiveAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        public Task DisconnectAsync(CancellationToken cancellationToken)
        {
            return _webSocketClient.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnect", cancellationToken);
        }

        public async Task ReceiveAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    string message = await ReceiveMessageAsync(cancellationToken);
                    MessageReceived?.Invoke(this, message);
                }
                catch (WebSocketException)
                {
                    _webSocketClient.Abort();
                    ConnectionClosed?.Invoke(this, null);
                    return;
                }
            }
        }

        /// <summary>
        /// Receives raw message from the opened connection.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<string> ReceiveMessageAsync(CancellationToken cancellationToken)
        {
            // RFC 1459 uses 512 bytes to hold one full message, therefore, it should be enough
            var byteArray = new byte[512];
            var receiveBuffer = new ArraySegment<byte>(byteArray);

            var receivedResult = await _webSocketClient.ReceiveAsync(receiveBuffer, cancellationToken);

            var msgBytes = receiveBuffer.Skip(receiveBuffer.Offset)
                .Take(receivedResult.Count)
                .ToArray();

            var receivedMessage = Encoding.UTF8.GetString(msgBytes);

            return receivedMessage;
        }
    }
}