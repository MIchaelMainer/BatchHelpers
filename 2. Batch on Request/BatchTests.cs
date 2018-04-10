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
            var part1 = graphClient.Me.BatchRequest(batch).Get();
            UserBatchPart part2 = graphClient.Users["admin@M365x462896.onmicrosoft.com"].BatchRequest(batch).Get();

            batch = await batch.PostAsync();
            User user = part1.Response;
        }
    }
}
