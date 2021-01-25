using System;
using System.Text.Json;
using TwitchInteraction.CrowdControl;

namespace TwitchInteraction
{
    internal class CrowdControlEventManager
    {
        internal static void MessageReceived(int sender, string message)
        {

            Console.WriteLine("Message received. Id: " + sender + ", message:" + message);
        }

        internal static void ClientConnected(CrowdControlClient c)
        {
            Console.WriteLine("Client connected");
            Console.WriteLine("Awaiting CC Command");

            c.Receive();            
        }

        internal static void ClientMessageReceived(CrowdControlClient c, string raw)
        {
            try { 
                Console.WriteLine("Message received: " + raw);
                // Decode the message from CC
                // { "id":1,"code":"kill","viewer":"sdk","type":1}
                var request = JsonSerializer.Deserialize<CrowdControlRequest>(raw);

                // Do the thing
                // TODO

                // Send the result to CC
                var response = new CrowdControlResponse();
                response.Id = request.Id;
                response.Status = 0;
                response.Message = "Effect: " + request.Code + ": " + response.Status;

                var resJson = JsonSerializer.Serialize<CrowdControlResponse>(response);
                Console.WriteLine("Sending response: " + resJson);

                c.Send(resJson, false);
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        internal static void ClientMessageSent(CrowdControlClient c, bool close)
        {
            Console.WriteLine("Message Sent.");
            // Await next command
            c.Receive();
        }
    }
}