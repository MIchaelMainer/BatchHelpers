using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Graph.Models
{
    public class BatchPart
    {
        public string Url { get; set; }

        public int Id { get; set; }

        public string Method { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "body", Required = Newtonsoft.Json.Required.Default)]
        public object Body { get; set; }

        [JsonIgnore]
        public Type ReturnType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "dependsOn", Required = Newtonsoft.Json.Required.Default)]
        public int[] DependsOn { get; set; }

        [JsonIgnore]
        public IBaseClient Client { get; set; }

        /// <summary>
        /// Create a new batch part
        /// </summary>
        /// <param name="id">The ID of the batch part</param>
        /// <param name="url">The URL of the request</param>
        /// <param name="method">The method to use (GET, POST)</param>
        /// <param name="body">The body of the request</param>
        /// <param name="dependsOn">A list of requests that this request depends on</param>
        public BatchPart(int id, string url, string method, object body = null, int[] dependsOn = null)
        {
            this.Id = id;
            this.Url = url;
            this.Method = method;
            this.Body = body;
            this.DependsOn = dependsOn;
        }
    }
}
