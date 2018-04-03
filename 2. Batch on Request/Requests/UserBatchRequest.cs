using Microsoft.Graph.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Graph.Requests
{
    /// <summary>
    /// The User batch request
    /// </summary>
    public class UserBatchRequest
    {
        /// <summary>
        /// The request URL
        /// </summary>
        public String Url { get; set; }

        public string BaseUrl { get; set; }

        [JsonIgnore]
        public IBaseClient client { get; set; }

        /// <summary>
        /// The batch for the request
        /// </summary>
        public Batch Batch { get; set; }

        /// <summary>
        /// Create a new user batch request
        /// </summary>
        /// <param name="batch">The batch to attach the request to</param>
        /// <param name="url">The URL of the request</param>
        /// <param name="client">The client to send the request with</param>
        /// <param name="options">The options for the request</param>
        public UserBatchRequest(Batch batch, String url, IBaseClient client, IEnumerable<Option> options = null)
        {
            this.Url = url;
            this.Batch = batch;
            this.BaseUrl = client.BaseUrl;
            this.client = client;
        }

        /// <summary>
        /// Call GET on the User through batch
        /// </summary>
        /// <returns></returns>
        public BatchPart Get()
        {
            var id = Batch.GetNextId();
            var url = this.Url.Replace(this.BaseUrl, "");
            BatchPart part = new BatchPart(id, url, "GET");
            part.Client = client;
            part.ReturnType = typeof(User);
            Batch.Add(part);
            return part;
        }
    }
}
