using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Graph.Requests
{
    public class BatchPart
    {
        public int id { get; set; }

        public string url { get; set; }

        public string method { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "body", Required = Newtonsoft.Json.Required.Default)]
        public object body { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "headers", Required = Newtonsoft.Json.Required.Default)]
        public object headers { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "headers", Required = Newtonsoft.Json.Required.Default)]
        //public IEnumerable<HeaderOption> headers { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "dependsOn", Required = Newtonsoft.Json.Required.Default)]
        public int[] dependsOn { get; set; }

        public BatchPart(int id, string url, string method, object body, int[] dependsOn)
        {
            this.id = id;
            this.url = url;
            this.method = method;
            this.body = body;
            this.dependsOn = dependsOn;
            
            if (body != null)
            {
                this.headers = new Dictionary<String, String>() { { "Content-Type", "application/json" } };
            }
        }

        [JsonIgnore]
        public IBaseRequest requestItem { get; set; }
    }
}
