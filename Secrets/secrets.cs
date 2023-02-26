using SMLHelper.V2.Handlers;
using System.Collections.Generic;

namespace TwitchInteraction
{
    public class Secrets
    {
        public string client;
        public string client_id;
        public string access_token;
        public string api_token;
        public string nick_id;
        public string username;
        public string botname;
        public bool showRedemptionMessages;
        public bool saveRedemptionMessages;

        public List<ConfigEventInfo> eventConfigList = new List<ConfigEventInfo>();

        public Secrets()
        {
            MainPatcher.LogSource.LogInfo("Loading config file");
            Config config = OptionsPanelHandler.Main.RegisterModOptions<Config>();
            EventsFile events = OptionsPanelHandler.Main.RegisterModOptions<EventsFile>();

            config.Load();
            MainPatcher.LogSource.LogInfo("Loaded config file");
            if (config.Client != null)
            {
                client = config.Client;
            } else
            {
                client = "twitch";
            }

            client_id = config.ClientId;
            access_token = config.BotAccessToken;
            api_token = config.UsernameToken;
            nick_id = config.UsernameId;
            username = config.Username;
            botname = config.BotName;
            showRedemptionMessages = config.ShowRedemptionMessages;
            saveRedemptionMessages = config.SaveRedemptionMessages;

            events.Load();
            events.UpdateEventsData();
        }
    }
}