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
            IGraphServiceUsersCollectionRequest newUserRequest = graphClient.Users.Request();                               // We have the ./users URL, query parameters, and request headers.

            User newUser = new User();
            newUser.DisplayName = "New User";
            newUser.UserPrincipalName = "newuser@M365x462896.onmicrosoft.com";

            IDirectoryObjectWithReferenceRequest managerRequest = graphClient.Me.Manager.Request();                         // We have the /me/manager URL, query parameters, and request headers.
            IDriveItemChildrenCollectionRequest driveRequest = graphClient.Me.Drive.Root.Children.Request();                // We have the /me/drive/root/children URL, query parameters, and request headers.

            IEducationRootRequest eduRequest = graphClient.Education.Request();                                             // We have the /education URL, query parameters, and request headers.

            BatchContainer batchContainer = new BatchContainer();
            BatchPart part1 = batchContainer.AddToBatch(meRequest, Method.GET);                                             // I don't think we need a copy of the BatchPart.
            batchContainer.AddToBatch(driveRequest, Method.GET);
            batchContainer.AddToBatch(eduRequest, Method.GET);
            batchContainer.AddToBatch(newUserRequest, Method.POST, 4, newUser, new BatchPart[] { part1 });                  // We have to use reflection to determine which HttpVerb method we are using, and then, what
                                                                                                                            // the return type will be. This might be costly batch scenario can contain a large number 
            BatchResponse response = await graphClient.Batch.PostAsync(batchContainer);                                     // of requests across many batches. I think we want to avoid reflection. 

            User me = (User)response.batchResponses.Where(i => i.id == 1).First().body;                                     // No auto-deserialization.

            User me2 = (User)response.batchResponses.Where(i => i.body.GetType() == typeof(User)).FirstOrDefault().body;

            foreach (BatchResponsePart part in response.batchResponses)
            {
                var responseItem = part.body; // If we deserialize into a dynamic object, the customer would have 
                int statusCode = part.status;
            }

            Assert.IsNotNull(me.UserPrincipalName);
        }
    }
}