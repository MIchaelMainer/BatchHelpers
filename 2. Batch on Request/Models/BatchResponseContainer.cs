using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Graph.Models
{
    /// <summary>
    /// Batch response which holds the set of individual responses
    /// </summary>
    public class BatchResponseContainer
    {
        /// <summary>
        /// Set of the individual responses
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "responses", Required = Newtonsoft.Json.Required.Default)]
        public IEnumerable<BatchResponsePart> batchResponses { get; set; }
    }
}
