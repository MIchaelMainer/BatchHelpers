using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Graph.Requests
{
    public class BatchRequest
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "body", Required = Newtonsoft.Json.Required.Default)]
        public object Body { get; set; }

        public string Url { get; set; }

        public string Method { get; set; }

        public BatchRequest()
        {

        }
    }
}
