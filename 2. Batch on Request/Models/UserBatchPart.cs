using Microsoft.Graph.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Graph.Models
{
    public class UserBatchPart : BatchPart
    {
        [JsonIgnore]
        new public User Response { get; set; }

        public UserBatchPart(BatchRequest batchRequest, int id, string url, string method, object body = null, int[] dependsOn = null):
            base(batchRequest, id, url, method, body, dependsOn)
        {

        }
    }
}
