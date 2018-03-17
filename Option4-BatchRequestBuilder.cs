// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  
//  See License in the project root for license information.
// ------------------------------------------------------------------------------

/**
Option 4: Batch request builder

Use the existing request builders to build up the request URL, query options, and request headers. 
You can build multiple batches at a time. This option shows two way you can create a BatchPart:
a) Build a BatchPart from the cstor.
b) Build a BatchPart from request builders. This is something we can generate.

**/

// Build BatchParts using a cstor.
var driveToFetch = graphClient.Me.Drive.Request();
var groupToAdd = graphClient.Groups.Request();
//specify request, verb, custom headers, body if exists (overloads), expected return type.
BatchPart batchPart1 = new BatchPart("get", driveToFetch, "1"); 
BatchPart batchPart2 = new BatchPart("post", groupToAdd, groupBody, "2", ["1"]);

// Build BatchParts using requestbuilder. 
// We can generate this since is matches the existing call, for example:
// User user = await graphClient.Users.Request(query).PostAsync(new User);
// Since we know what the return type is, we have the potential to provide
// type hints to automate deserialization.  
// Query parameters and headers are set in Request(). That gives us a complete
// URL and headers collection.
// The contents of BatchPart body are set {HttpVerb}BatchPart method. 
// The BatchPart id will automatically be set by default if it is not provided.
// DependsOn references can be set as part of a param[].
BatchPart batchPart3 = graphClient.Users.Request(query).PostBatchPart(new User, two, ?id);

// We have the return type at compilation. We can identify it and generate
// from our templates.
Type batchPart3ResponseType = batchPart3.GetResponseBodyType();

// Create our intermediate BatchRequestCollection object. We can create multiple batches
// at a time. 
BatchRequestCollection batch = new BatchRequestCollection();

// Check if safe to add parts
if (batch.spaceRemaining() >= 3)
{ 
	batch.addPart(batchPart1);
    batch.addPart(batchPart2);
	batch.addPart(batchPart3);
}

// Send the batch request with headers and query params set on Options.
BatchResponse res = await graphClient.BatchRequestsAsync(batch, new Options());

// Access the parts of the response.
HttpHeaders headers = res.Headers;
HttpStatusCode statusCode = res.StatusCode;
string rawBody = res.RawBody;

Collection<BatchResponsePart> parts = res.BatchResponseParts;
Serializer serializer = new Serializer();

foreach (BatchResponsePart part in parts)
{
    // Each part would have the type parameter defined so we can remove the need to cast it.
    T myObject = part.GetDeserializedObject<T>();
    HttpHeaders partHeaders = part.Headers;
    HttpStatusCode partStatusCode = part.StatusCode;
    int partId = part.Id;
    string partBody = part.RawBody;
}

/* Open questions for Option 3.
What happens if one of the batch response parts is a redirect?
What is the experience if one of the requests requires a scope that is not granted? This would be like any other expected Graph error scenario.
We need to investigate whether we can maintain references well across making the request.
We need to know the batching roadmap so we are at least considering some future features.
Would it be useful to maintain the dependsOn relationship on the response? Because that is not returned in the protocol.
*/