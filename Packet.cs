using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskSubscriber
{
    class Packet
    {
        [JsonIgnore]
        public int id { get; set; }

        [JsonProperty("id")]
        public int publishId { get; set; }
        public string message { get; set; }
        public DateTime sendDate { get; set; }
        public string hash { get; set; }

        [JsonIgnore]
        public DateTime receiveDate { get; set; }
    }
}
