using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace Microsoft.Graph.Requests
{
    public class BatchContainer
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "requests", Required = Newtonsoft.Json.Required.Default)]
        public IEnumerable<BatchPart> batchRequestItems { get; set; }

        public BatchContainer() {
            this.batchRequestItems = new List<BatchPart>();
        }

        public BatchPart AddToBatch(IBaseRequest requestItem, Method method, int id = -1, object body = null, BatchPart[] dependsOn = null)
        {
            int batchId;
            if (id == -1)
            {
                if (batchRequestItems.Count() == 0)
                {
                    batchId = 1;
                } else
                {
                    batchId = batchRequestItems.Count() + 1;
                }
                
            } else
            {
                batchId = id;
            }

            var batchUrl = requestItem.RequestUrl.Replace(":443", "");
            batchUrl = batchUrl.Replace(requestItem.Client.BaseUrl, "");

            int[] dependsOnIds = null;
            if (dependsOn != null)
            {
                dependsOnIds = dependsOn.Select(i => i.id).ToArray();
            }

            BatchPart request = new BatchPart(batchId, batchUrl, method.ToString(), body, dependsOnIds);
            request.requestItem = requestItem;

            batchRequestItems = batchRequestItems.Concat(new List<BatchPart> { request });
            return request;
        }
    }
}
