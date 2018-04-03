using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Graph.Requests
{
    public class BatchResponse
    {
        public Dictionary<String, String> headers { get; set; }

        public int status { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "responses", Required = Newtonsoft.Json.Required.Default)]
        public IEnumerable<BatchResponsePart> batchResponses { get; set; }
    }
}
