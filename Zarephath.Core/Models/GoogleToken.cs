using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarephath.Core.Models
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class GoogleToken
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; internal set; }

        [JsonProperty(PropertyName = "expires_in")]
        public int Expires { get; internal set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; internal set; }

        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; internal set; }

        [JsonProperty(PropertyName = "authentication_token")]
        public string AuthenticationToken { get; internal set; }

        [JsonProperty(PropertyName = "scope")]
        public string Scope { get; internal set; }
    }
}
