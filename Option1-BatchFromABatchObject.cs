/**
Option 1: Batch from a batch object

Use the existing request builders to build up thee request URL, query options, and request headers. The request
builders are not used for adding the request body.

**/

// The following lines build up the request URLs, and add query parameters and request headers.
IUserRequest meRequest = graphClient.Me.Request();
IDriveItemChildrenCollectionRequest driveRequest = graphClient
                 .Me
                 .Drive
                 .Root
                 .Children
                 .Request();

// The BatchContainer adds the requests to the collection. You have set the required HttpVerbs
// and the optional request body, optional Id property (in case we auto assign if not explicitly set),
// and the DependsOn property in the AddToBatch methods. For the future, we can imagine a piping syntax
// as well so consider that this method signature could expand for the scenario where dependsOn is used.
// Imagine a current signature like this:
// void AddToBatch(IBaseRequest request, HttpMethod method, string body = "", string id = "", string dependsOn = "")
BatchContainer batchContainer = new BatchContainer();
batchContainer.AddToBatch(meRequest, Method.GET, id: "1");
batchContainer.AddToBatch(driveRequest, Method.GET, id: "2");

// Send the batched requests and get the batch response.
BatchResponseContainer response = await graphClient
               .Batch
               .PostAsync(batchContainer);

// Access the response envelope.
HttpHeaders responseHeaders = response.Headers;
HttpStatusCode responseStatus = response.HttpStatusCode;
string rawResponseBody = response.RawResponseBody;

Serializer serializer = new Serializer();

User me1;
ServiceException ex1;

// odata.type is not guaranteed to be returned. So, the user needs know which batch part id correlates
// to the type of object returned in response. And since the order is not guaranteed, we have to check id.
foreach(BatchResponsePart part in response.BatchResponseParts)
{
    // We know that we should try to deserialize the identified batch since we are assumed to know
    // that the call for batch part 1 is supposed to return a User.
    if (part.Id = "1" && part.IsSuccessStatusCode)
    {
        // Get a strongly typed object.
        me1 = serializer.DeserializeObject<User>(part.Body;)        
        // Be ready to handle serialization errors. How do we handle that so that the other batch parts can continue?
    }
    if (part.Id = "1" && !part.IsSuccessStatusCode)
    {
        // Get a strongly typed object.
        ex1 = serializer.DeserializeObject<ServiceException>(part.Body;)        
        // Be ready to handle serialization errors. How do we handle that so that the other batch parts can continue?
    }

    // continue this for each batch part.
}

// We can't use the following as we don't know whether the batch part was a success or not.
// User me = serializer.DeserializeObject<User>(response.batchResponses.Where(i => i.id == 1).First().body.ToString());

/* Open questions for Option 1.
What happens if one of the batch response parts is a redirect?
What is the experience if one of the requests requires a scope that is not granted? This would be like any other expected Graph error scenario.
Do we need intermediate batch part objects? We can't automate a reference tree without them.
*/