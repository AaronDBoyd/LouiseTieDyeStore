using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LouiseTieDyeStore.Shared
{
    public class OidcUserSessionToken
    {
        public string id_token { get; set; } = string.Empty;
        public string access_token {  get; set; } = string.Empty;
        public string token_type { get; set; } = string.Empty;
        public string scope { get; set; } = string.Empty;

        [JsonIgnore]
        public object profile {  get; set; }

        public int expires_at { get; set; }
    }
}
