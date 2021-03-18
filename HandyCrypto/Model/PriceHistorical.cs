using System.Collections.Generic;
using Newtonsoft.Json;

namespace HandyCrypto.Model
{
    public class PriceHistorical
    {
        [JsonProperty("Coin")]
        public Dictionary<string, decimal> Coin { get; set; }
    }
}