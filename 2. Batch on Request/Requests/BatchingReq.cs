using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using Newtonsoft.Json;

namespace Microsoft.Graph.Requests
{
    public class BatchingReq : BaseRequest
    {
        public BatchingReq(
            string requestUrl,
            IBaseClient client,
            IEnumerable<Option> options)
            : base(requestUrl, client, options)
        {
        }

        public async System.Threading.Tasks.Task<Batch> PostAsync(Batch batch)
        {
            this.ContentType = "application/json";
            this.Method = "POST";

            //Currently the requests are being serialized into a sub-array since they are a separate object
            //Need to write a custom JSON.NET rule to parse this correctly
            var res = await this.SendAsync<Batch>(batch, CancellationToken.None);

            foreach (var resItem in res.BatchItems)
            {
                var tempBody = JsonConvert.DeserializeObject<Dictionary<string, object>>(resItem.Response.ToString());
                if (tempBody.ContainsKey("value"))
                {
                    resItem.Response = this.Client.HttpProvider.Serializer.DeserializeObject(tempBody["value"].ToString(), resItem.Response.GetType());
                }
                else
                {
                    resItem.Response = this.Client.HttpProvider.Serializer.DeserializeObject(resItem.Response.ToString(), resItem.Response.GetType());
                }
            }

            return res;
        }
    }
}
