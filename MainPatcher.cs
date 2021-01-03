using System.Reflection;
using TwitchLib.Client;
using TwitchLib.Unity;
using TwitchLib.PubSub;
using System;
using Harmony;

namespace TwitchInteraction
{
    public class MainPatcher
    {
        public static TwitchChatClient otherclient;
        public static TwitchPubSubClient otherpubsub;
        public static Channel TextChannel;
        public static Channel PubSubChannel;
        public static System.Threading.CancellationToken cts;
        public static System.Threading.CancellationToken cts2;
        public static Api api;
        public static Secrets secrets;
        
        public static void Patch()
        {
            secrets = new Secrets();
            StartTwitchChatClient();
            StartTwitchPubSubClient();

            var harmony = HarmonyInstance.Create("subnautica.mod.twitchinteraction"); 
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        private static async void StartTwitchChatClient()
        {
            cts = new System.Threading.CancellationToken();
            otherclient = new TwitchChatClient();
            await otherclient.ConnectAsync("oauth:" + secrets.access_token, secrets.botname, cts);
            TextChannel = await otherclient.JoinChannelAsync(secrets.username, cts);
            TextChannel.MessageReceived += TwitchEventManager.ChatMessageReceived;
        }

        private static async void StartTwitchPubSubClient()
        {
            cts2 = new System.Threading.CancellationToken();
            otherpubsub = new TwitchPubSubClient();
            await otherpubsub.ConnectAsync(secrets.api_token, secrets.nick_id, cts2);
            PubSubChannel = await otherpubsub.JoinChannelAsync(secrets.username, cts);
            PubSubChannel.MessageReceived += TwitchEventManager.PubSubMessageReceived;
        }
    }



}