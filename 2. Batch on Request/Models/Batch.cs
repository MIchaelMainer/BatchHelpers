using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph.Requests;
using Newtonsoft.Json;

namespace Microsoft.Graph.Models
{
    public class Batch
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "requests", Required = Newtonsoft.Json.Required.Default)]
        public IEnumerable<BatchPart> BatchItems { get; set; }

        public Batch()
        {
            this.BatchItems = new List<BatchPart>();
        }

        public async Task<Batch> PostAsync()
        {
            if (BatchItems.Count() == 0)
            {
                return null;
            }
            var sacrificialItem = BatchItems.First();
            var client = sacrificialItem.Client;
            var url = sacrificialItem.Client.BaseUrl;
            BatchingReq req = new BatchingReq(url + "/$batch", client, null);
            return await req.PostAsync(this);
        }

        public void Add(BatchPart batchItem)
        {
            BatchItems = BatchItems.Concat(new List<BatchPart> { batchItem });
        }

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
    }
}
