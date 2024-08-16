using BepInEx;
using BepInEx.Logging;
using System.Reflection;
using TwitchInteraction.CrowdControl;
using System;
using HarmonyLib;

namespace TwitchInteraction
{

    [BepInPlugin(myGUID, pluginName, versionString)]

    public class MainPatcher: BaseUnityPlugin
    {
        public const string myGUID = "TwitchInteractionMod";
        public const string pluginName = "TwitchInteraction";
        public const string versionString = "1.0.0";

        public static TwitchChatClient otherclient;
        public static TwitchPubSubClient otherpubsub;
        public static Channel TextChannel;
        public static Channel PubSubChannel;
        public static System.Threading.CancellationToken cts;
        public static System.Threading.CancellationToken cts2;
        public static Secrets secrets;
        internal static ManualLogSource LogSource { get; private set; }
        internal static Assembly myAssembly = Assembly.GetExecutingAssembly();

        public void Awake()
        {
            LogSource = base.Logger;
            LogSource.LogInfo("TwitchInteraction -- Awake");
            secrets = new Secrets();
            LogSource.LogInfo("TwitchInteraction -- Secrets loaded");

            if (secrets.client == "crowdcontrol")
            {
                Console.WriteLine("CrowdControl client active");
                StartCrowdControlServer();

            } else {

                Console.WriteLine("Twitch client active");
                //StartTwitchChatClient(); Turned off cause the ping pong doesnt work and when it disconnects it crashes the game
                StartTwitchPubSubClient();
            }

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
		
        private static void StartCrowdControlServer()
	{
            var client = new CrowdControlClient();

            // Setup handlers
            client.Connected += new ConnectedHandler(CrowdControlEventManager.ClientConnected);
            client.MessageReceived += new ClientMessageReceivedHandler(CrowdControlEventManager.ClientMessageReceived);
            client.MessageSubmitted += new ClientMessageSubmittedHandler(CrowdControlEventManager.ClientMessageSent);

            client.StartClient();
        }
    }
}
