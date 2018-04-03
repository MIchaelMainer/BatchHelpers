using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Graph.Requests;

namespace Microsoft.Graph.Models
{
    /// <summary>
    /// Batch
    /// </summary>
    public class Batch
    {
        /// <summary>
        /// Batch items
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "requests", Required = Newtonsoft.Json.Required.Default)]
        public IEnumerable<BatchPart> BatchItems { get; set; }

        public string Status { get; set; }

        /// <summary>
        /// Create a new batch
        /// </summary>
        public Batch()
        {
            this.BatchItems = new List<BatchPart>();
        }

        /// <summary>
        /// Add an item to a batch
        /// </summary>
        /// <param name="batchItem"></param>
        public void Add(BatchPart batchItem)
        {
            BatchItems = BatchItems.Concat(new List<BatchPart> { batchItem });
        }

        /// <summary>
        /// Get the next available ID for the batch
        /// </summary>
        /// <returns></returns>
        public int GetNextId()
        {
            int batchId;
            if (BatchItems.Count() == 0)
            {
                batchId = 1;
            }
            else
            {
                batchId = BatchItems.Count() + 1;
            }
            return batchId;
        }

        /// <summary>
        /// POST the batch to Graph
        /// This will use the client from the first request in the batch.
        /// It does not support both endpoints (both beta and v1.0 calls)
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<BatchResponseContainer> PostAsync()
        {
            if (BatchItems.Count() == 0)
            {
                return null;
            }
            var sacrificialItem = BatchItems.First();
            var client = sacrificialItem.Client;
            var url = sacrificialItem.Client.BaseUrl;
            BatchRequest req = new BatchRequest(url + "/$batch", client, null);
            return await req.PostAsync(this);
        }
    }
}
