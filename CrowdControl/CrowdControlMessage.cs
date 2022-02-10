namespace TwitchInteraction.CrowdControl
{
    public class CrowdControlRequest
    {
        public int id { get; set; }

        public string code { get; set; }

        public string viewer { get; set; }

        public int type { get; set; }
    }

    public class CrowdControlResponse
    {
        public int id { get; set; }

        // status is 0: success, 1: failure, 2: unavailable, 3: retry
        public int status { get; set; }

        public string message { get; set; }
    }
}