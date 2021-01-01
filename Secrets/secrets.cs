using System.IO;
using System.Reflection;
using System.Text.Json;


namespace TwitchInteraction
{
    public class Secrets
    {

        private static string _configFilePath = null;
        private static string ConfigFilePath { get => _configFilePath ?? (_configFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Config.txt")); }



        public string client_id;
        public string access_token;
        public string api_token;
        public string nick_id;
        public string username;
        public string botname;

        public Secrets()
        {
            Config config = JsonSerializer.Deserialize<Config>(File.ReadAllText(ConfigFilePath));
            client_id = config.ClientId;
            access_token = config.BotAccessToken;
            api_token = config.UsernameToken;
            nick_id = config.UsernameId;
            username = config.Username;
            botname = config.BotName;
        }
    }
}