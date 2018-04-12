using Microsoft.Graph.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Graph.Test.Requests.Functional
{
    [TestClass]
    public class BatchTests : GraphTestBase
    {
        [TestMethod]
        public async System.Threading.Tasks.Task TestBatch()
        {
            var batch = new Batch();
            BatchPart req1 = graphClient.Me.BatchRequest(batch).Get();                          // We'd know the return type  and verb at generation. The client get sets from this.
            graphClient.Users["admin@M365x462896.onmicrosoft.com"].BatchRequest(batch).Get();

            BatchResponseContainer response = await batch.PostAsync();

            foreach (var item in response.batchResponses)                                       // What is item?
            {
                var user = item.Body;
                Assert.AreEqual(200, item.Status);
            }
            Assert.IsNotNull(response);
        }
    }
}
