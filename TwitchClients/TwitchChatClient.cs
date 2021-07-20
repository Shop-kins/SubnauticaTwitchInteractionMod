using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TwitchInteraction
{
    public class TwitchChatClient
    {

        public event EventHandler ConnectionClose;

        private IMessageClient _twitchMessageClient;

        private MessageParser _parser = new MessageParser();

        protected ICollection<Channel> _channels = new List<Channel>();

        public TwitchChatClient()
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
        private async void OnRawMessageReceived(object sender, string e)
        {
            // About once every five minutes, the server sends a PING.
            // To ensure that your connection to the server is not prematurely terminated, reply with PONG
            if (e.StartsWith("PING"))
            {
                await SendPongResponseAsync();
                return;
            }

            if (_parser.TryParsePrivateMessage(e, out Message message))
            {
                Channel c = _channels.FirstOrDefault(d => d.Name == message.Channel);
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
        public Task ConnectAsync(string oauth, string nick, CancellationToken cancellationToken)
        {
            if (!oauth.StartsWith("oauth:"))
            {
                throw new ArgumentException("OAuth parameter must in format 'oauth:xxxx'");
            }

            _twitchMessageClient = new WebSocketMessageClient();
            _twitchMessageClient.MessageReceived += OnRawMessageReceived;
            _twitchMessageClient.ConnectionClosed += OnConnectionClosed;

            return _twitchMessageClient.ConnectAsync(oauth, nick.ToLower(), cancellationToken);
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
            await _twitchMessageClient.SendMessageAsync($"JOIN #{cn}", cancellationToken);
            var channel = new Channel(cn, _twitchMessageClient);
            _channels.Add(channel);

            return channel;
        }

        /// <summary>
        /// About once every five minutes, the server will send a PING :tmi.twitch.tv. 
        /// To ensure that your connection to the server is not prematurely terminated, reply with PONG :tmi.twitch.tv.
        /// </summary>
        /// <returns></returns>
        private Task SendPongResponseAsync()
        {
            return _twitchMessageClient.SendMessageAsync("PONG :tmi.twitch.tv", CancellationToken.None);
        }
    }
}