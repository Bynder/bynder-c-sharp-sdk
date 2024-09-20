using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bynder.Sdk.Model
{
    public class MediaFullResult
    {
        [JsonProperty("total")]
        public Total Total { get; set; }

        [JsonProperty("media")]
        public IList<Media> Media { get; set; }
    }
    public class Total
    {
        public long Count { get; set; }
    }
}
