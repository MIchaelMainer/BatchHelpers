using Microsoft.Graph.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Graph.Models
{
    public class BatchPart
    {
        //Currently the requests are being serialized into a sub-array since they are a separate object
        //Need to write a custom JSON.NET rule to parse this correctly
        public BatchRequest Request { get; set; }

        [JsonIgnore]
        public object Response { get; set; }

        public int Id { get; set; }

        public int[] DependsOn { get; set; }

        [JsonIgnore]
        public IBaseClient Client { get; set; }

        public BatchPart(BatchRequest batchRequest, int id, string url, string method, object body = null, int[] dependsOn = null)
        {
            this.Id = id;
            this.Request = batchRequest;
            this.Request.Url = url;
            this.Request.Method = method;
            this.Request.Body = body;
        }
    }
}
