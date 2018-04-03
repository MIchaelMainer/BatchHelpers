using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

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

        public async System.Threading.Tasks.Task<BatchResponse> PostAsync(BatchContainer batch)
        {
            this.ContentType = "application/json";
            this.Method = "POST";
            BatchResponse res = await this.SendAsync<BatchResponse>(batch, CancellationToken.None);

            //Serializer serializer = new Serializer();

            foreach (var resItem in res.batchResponses)
            {
                var matchingReq = batch.batchRequestItems.Where(y => y.id == resItem.id).FirstOrDefault();

                var method = matchingReq.method;

                var methodName = GetAsyncMethodName(method);

                var t = matchingReq.requestItem.GetType();
                var returnMethod = t.GetRuntimeMethods().Where(x => x.Name == methodName).FirstOrDefault();
                var z = t.GetRuntimeMethods();
                var typeName = returnMethod.ReturnType.GenericTypeArguments.FirstOrDefault().FullName;
                Type newType = Type.GetType(typeName);

                var tempBody = JsonConvert.DeserializeObject<Dictionary<string, object>>(resItem.body.ToString());
                if (tempBody.ContainsKey("value"))
                {
                    resItem.body = this.Client.HttpProvider.Serializer.DeserializeObject(tempBody["value"].ToString(), newType);
                } else
                {
                    resItem.body = this.Client.HttpProvider.Serializer.DeserializeObject(resItem.body.ToString(), newType);
                }

                
            }
            return res;
        }

        public String GetAsyncMethodName(string method)
        {
            switch (method)
            {
                case "GET":
                    return "GetAsync";
                case "POST":
                    return "AddAsync";
                case "DELETE":
                    return "DeleteAsync";
                case "PUT":
                    return "CreateAsync";
                default:
                    return "GetAsync";
            }
        }
    }
}
