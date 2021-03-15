using System;
using System.Text.Json.Serialization;

namespace TwitchInteraction.CrowdControl
{
    public class CrowdControlRequest
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("viewer")]
        public string Viewer { get; set; }

        [JsonPropertyName("type")]
        public int Type { get; set; }
    }

    public class CrowdControlResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        // status is 0: success, 1: failure, 2: unavailable, 3: retry
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}