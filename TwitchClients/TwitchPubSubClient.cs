using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TwitchInteraction
{
    public class TwitchPubSubClient
    {

        public event EventHandler ConnectionClose;

        private IMessageClient _twitchMessageClient;

        private PubSubParser _parser = new PubSubParser();

        protected ICollection<Channel> _channels = new List<Channel>();

        public TwitchPubSubClient()
        {
        }

        /// <summary>
        /// Triggered when the connection is closed from any reason.
        /// </summary>
        /// <param name="sender">IMessageClient</param>
        /// <param name="e">null</param>
        private void OnConnectionClosed(object sender, EventArgs e)
        {
            ConnectionClose?.Invoke(this, e);
        }

        /// <summary>
        /// Triggered when raw message received.
        /// </summary>
        /// <param name="sender">IMessageClient</param>
        /// <param name="e">Raw message</param>
        private void OnRawMessageReceived(object sender, string e)
        {
            if (_parser.TryParsePrivateMessage(e, out var message))
            {
                var c = _channels.FirstOrDefault(d => d.Name == message.Channel);
                c?.ReceiveMessage(message);
            }
        }

        /// <summary>
        /// Opens a connection to the server and start receiving messages.
        /// </summary>
        /// <param name="oauth">Your password should be an OAuth token authorized through our API with the chat:read scope (to read messages) and the  chat:edit scope (to send messages)</param>
        /// <param name="nick">Your nickname must be your Twitch username (login name) in lowercase</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ConnectAsync(string oauth, string nickId, CancellationToken cancellationToken)
        {

            _twitchMessageClient = new WebSocketPubSubClient();
            _twitchMessageClient.MessageReceived += OnRawMessageReceived;
            _twitchMessageClient.ConnectionClosed += OnConnectionClosed;

            return _twitchMessageClient.ConnectAsync(oauth, nickId.ToLower(), cancellationToken);
        }

        /// <summary>
        /// Joins to given twitch channel. Connection must be established first.
        /// </summary>
        /// <param name="channelName">A channel name</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Channel> JoinChannelAsync(string channelName, CancellationToken cancellationToken)
        {
            var cn = channelName.ToLower();
            var channel = new Channel(cn, _twitchMessageClient);
            _channels.Add(channel);

            return channel;
        }
    }
}