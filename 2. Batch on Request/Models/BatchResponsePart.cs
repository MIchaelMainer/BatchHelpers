using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Graph.Models
{
    public class BatchResponsePart
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Newtonsoft.Json.Required.Default)]
        public int Id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "status", Required = Newtonsoft.Json.Required.Default)]
        public int Status { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "headers", Required = Newtonsoft.Json.Required.Default)]
        //public IEnumerable<HeaderOption> headers { get; set; }
        private string _body;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "body", Required = Newtonsoft.Json.Required.Default)]
        public object Body { get; set; }

        public string RawBody { get; set; }
    }
}
