﻿using Newtonsoft.Json;
namespace BusinessLayer.Models.DTOs
{
    public class GeneratedTokenDTO
    {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("tokenType")]
        public string TokenType { get; set; } = "bearer";

        [JsonProperty("expiresIn")]
        public int ExpiresIn { get; set; }
    }
}
