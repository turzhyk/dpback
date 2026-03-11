using System.Text.Json.Serialization;

namespace DPBack.Application.Contracts
{
    public class PayUTokenReponseDto
    {   [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
}