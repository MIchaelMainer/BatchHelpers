using Microsoft.Graph.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Graph.Test.Requests.Functional
{
    [TestClass]
    public class BatchRequestTests : GraphTestBase
    {
        [TestMethod]
        public async System.Threading.Tasks.Task TestBatch()
        {
            IUserRequest meRequest = graphClient.Me.Request();
            IGraphServiceUsersCollectionRequest newUserRequest = graphClient.Users.Request();

            User newUser = new User();
            newUser.DisplayName = "New User";
            newUser.UserPrincipalName = "newuser@M365x462896.onmicrosoft.com";

            IDirectoryObjectWithReferenceRequest managerRequest = graphClient.Me.Manager.Request();
            IDriveItemChildrenCollectionRequest driveRequest = graphClient.Me.Drive.Root.Children.Request();

            IEducationRootRequest eduRequest = graphClient.Education.Request();

            BatchContainer batchContainer = new BatchContainer();
            BatchPart part1 = batchContainer.AddToBatch(meRequest, Method.GET);
            batchContainer.AddToBatch(driveRequest, Method.GET);
            batchContainer.AddToBatch(eduRequest, Method.GET);
            batchContainer.AddToBatch(newUserRequest, Method.POST, 4, newUser, new BatchPart[] { part1 });

            BatchResponse response = await graphClient.Batch.PostAsync(batchContainer);

            User me = (User)response.batchResponses.Where(i => i.id == 1).First().body;

            User me2 = (User)response.batchResponses.Where(i => i.body.GetType() == typeof(User)).FirstOrDefault().body;

            foreach (BatchResponsePart part in response.batchResponses)
            {
                var responseItem = part.body;
                int statusCode = part.status;
            }

            Assert.IsNotNull(me.UserPrincipalName);
        }
    }
}
