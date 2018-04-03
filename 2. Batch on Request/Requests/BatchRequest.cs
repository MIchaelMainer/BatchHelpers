using Microsoft.Graph.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Microsoft.Graph.Requests
{
    public class BatchRequest : BaseRequest
    {
        public BatchRequest(
            string requestUrl,
            IBaseClient client,
            IEnumerable<Option> options)
            : base(requestUrl, client, options)
        {
        }

        public async System.Threading.Tasks.Task<BatchResponseContainer> PostAsync(Batch batch)
        {
            this.ContentType = "application/json";
            this.Method = "POST";
            var res = await this.SendAsync<BatchResponseContainer>(batch, CancellationToken.None);

            foreach (var resItem in res.batchResponses)
            {
                var matchingReq = batch.BatchItems.Where(y => y.Id == resItem.Id).FirstOrDefault();

                var tempBody = JsonConvert.DeserializeObject<Dictionary<string, object>>(resItem.Body.ToString());
                if (tempBody.ContainsKey("value"))
                {
                    resItem.Body = this.Client.HttpProvider.Serializer.DeserializeObject(tempBody["value"].ToString(), matchingReq.ReturnType);
                }
                else
                {
                    resItem.Body = this.Client.HttpProvider.Serializer.DeserializeObject(resItem.Body.ToString(), matchingReq.ReturnType);
                }
            }

            return res;
        }
    }
}
