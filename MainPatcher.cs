using QModManager.API.ModLoading;
using System.Reflection;
using TwitchLib.Unity;
using TwitchLib.PubSub;
using System;
using HarmonyLib;
using TwitchInteraction.Player_Events;

namespace TwitchInteraction
{
    [QModCore]
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

        internal static Assembly myAssembly = Assembly.GetExecutingAssembly();

        [QModPatch]
        public static void Patch()
        {
            secrets = new Secrets();

            // Customize event configuration
            EventLookup.ConfigureEventCost(secrets.eventConfigList);

            //StartTwitchChatClient(); Turned off cause the ping pong doesnt work and when it disconnects it crashes the game
            StartTwitchPubSubClient();

            Harmony.CreateAndPatchAll(myAssembly, "subnautica.mod.twitchinteraction");
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