using System;
using System.Threading;
using System.Threading.Tasks;

namespace TwitchInteraction
{
    interface IMessageClient
    {
        /// <summary>
        /// Makes connection to the server and start receiving messages.
        /// </summary>
        /// <param name="oauth">Your password should be an OAuth token authorized through our API with the chat:read scope (to read messages) and the  chat:edit scope (to send messages)</param>
        /// <param name="nick">Your nickname must be your Twitch username (login name) in lowercase</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ConnectAsync(string oauth, string nick, CancellationToken cancellationToken);

        /// <summary>
        /// Disconnects from the server and stops receiving new messages.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DisconnectAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Send message to established connection.
        /// </summary>
        /// <param name="message">Message contant not longer then 512 bytes.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SendMessageAsync(string message, CancellationToken cancellationToken);

        /// <summary>
        /// Triggers when any message received.
        /// </summary>
        event EventHandler<string> MessageReceived;

        /// <summary>
        /// Triggers when connection closed for any reason.
        /// </summary>
        event EventHandler ConnectionClosed;
    }
}