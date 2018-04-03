using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Microsoft.Graph.Requests
{
    public class BatchResponsePart
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Newtonsoft.Json.Required.Default)]
        public int id { get; set;  }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "status", Required = Newtonsoft.Json.Required.Default)]
        public int status { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "headers", Required = Newtonsoft.Json.Required.Default)]
        //public IEnumerable<HeaderOption> headers { get; set; }
        private string _body;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "body", Required = Newtonsoft.Json.Required.Default)]
        public object body { get; set; }

        public string rawBody { get; set; }
    }
}
