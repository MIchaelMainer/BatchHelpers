using Microsoft.Graph.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Graph.Requests
{
    public class UserBatchRequest : BatchRequest
    {
        public String Url { get; set; }

        public string BaseUrl { get; set; }

        [JsonIgnore]
        public IBaseClient client { get; set; }

        /// <summary>
        /// The batch for the request
        /// </summary>
        ///
        [JsonIgnore]
        public Batch Batch { get; set; }

        public UserBatchRequest(Batch batch, String url, IBaseClient client, IEnumerable<Option> options = null) : base()
        {
            this.Url = url;
            this.Batch = batch;
            this.BaseUrl = client.BaseUrl;
            this.client = client;
        }

        public UserBatchPart Get()
        {
            var id = Batch.GetNextId();
            var url = this.Url.Replace(this.BaseUrl, "");
            UserBatchPart part = new UserBatchPart(this, id, url, "GET");
            part.Client = client;
            Batch.Add(part);
            return part;
        }
    }
}
