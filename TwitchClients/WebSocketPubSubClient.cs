using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;

namespace TwitchInteraction
{
    class WebSocketPubSubClient : IMessageClient
    {
        public event EventHandler<string> MessageReceived;

        public event EventHandler ConnectionClosed;

        private ClientWebSocket _webSocketClient = new ClientWebSocket();

        private readonly Uri _webSocketServerUri;

        public WebSocketPubSubClient(string webSocketServerUrl = "wss://pubsub-edge.twitch.tv:443")
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

        public async Task ConnectAsync(string token, string channelId, CancellationToken cancellationToken)
        {
            await _webSocketClient.ConnectAsync(_webSocketServerUri, cancellationToken);

            if (_webSocketClient.State == WebSocketState.Open)
            {
                ListenRequest lr = new ListenRequest();
                ListenRequestData lrd = new ListenRequestData();
                lrd.AuthToken = token;
                String[] lrdt = { "channel-points-channel-v1." + channelId, "channel-bits-events-v2." + channelId };
                lrd.Topics = lrdt;
                lr.Data = lrd;
                lr.Nonce = "lkjsdhfiusdagf";
                lr.Type = "LISTEN";
                String jlr = JsonSerializer.Serialize<ListenRequest>(lr);
                await SendMessageAsync(jlr, cancellationToken);

                var timer = new Timer(async (e) =>
                {
                    await SendMessageAsync("{\"type\":  \"PING\"}", cancellationToken);
                }, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));

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
                    var message = await ReceiveMessageAsync(cancellationToken);
                    MessageReceived?.Invoke(this, message);
                }
                catch (WebSocketException)
                {
                    if (_webSocketClient.State != WebSocketState.Open)
                    {
                        ConnectionClosed?.Invoke(this, null);
                        return;
                    }
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
            var byteArray = new byte[1536];
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