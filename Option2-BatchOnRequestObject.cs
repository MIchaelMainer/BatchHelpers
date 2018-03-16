// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------

/**
Option 2: Batch directly on the request object

Use the existing request builders to build up the request URL, query options, and request headers. The request
builders are not used for adding the request body.

**/

// Each part of the batch is a request object that mirrors the same request builder pattern for making individual calls. The Batch object
// would have an overload that allows you to add the GraphServiceClient, URL, and body for features not yet generated..
var batch = new Batch(new Options());
BatchPartReference request1Reference = client.Users["user"].Calendar.Events["id"].RequestWithBatch(batch, new Options()).Get();
BatchPartReference request2Reference = client.Users["user"].Messages["messageId"].RequestWithBatch(batch, new Options(), request1reference).Get();
BatchPartReference request3Reference = client.Users.RequestWithBatch(batch, new Options(), request1reference, new User()).Post();
// It might more sense to have the verb be set as an argument to RequestWithBatch. Options is where you set query params and headers.

try 
{
    // Send the batch request with a POST. Could extend ExecuteAsync to add more versatility here. For example, this might be a place
    // to add behaviors - like automatic redirect follows.
    BatchResponseContainer batchResult = await batch.ExecuteAsync();
    // We'll want to consider providing a custom call stack to make the Batch calls. 

    // Access the entire response envelope.
    HttpStatusCode overallStatus = batchResult.HttpStatusCode;
    HttpHeaders headers = batchResult.HttpHeaders;
    string rawResponseBody = batchResult.RawResponseBody;

    User user;

    // Sequential access to each batch result part.
    foreach(BatchResponsePart part in batchResult.Responses)
    {
        HttpStatusCode partStatus = part.HttpStatusCode;
        HttpHeaders partHeaders = part.HttpHeaders;
        string partId = part.Id;
        string partRawResponseBody = part.RawResponseBody;

        // This way, dev will have to know what type to deserialize the response body into.
        user = serializer.DeserializeObject<EventCollection>(part.RawResponseBody);    
    }

    // Alternate response idea. After await batch.ExecuteAsync().
    // Can we populate the BatchPartReference objects with the responses from batch.ExecuteAsync()?
    // If so, then we should be able to get the deserialized objects.
    if (request1Reference.Response != null) 
    {
        EventCollection events = request1Reference.Response.GetObject<EventCollection>();
    }
}
catch (ServiceException ex) // This will only be the case if the entire call fails.
{} 

/* Open questions for Option 2.
Does the final method represent the Http verb rather than a method action? If so, we should make this last call a property, or delete it
and make the verb a parameter in RequestWithBatch. 

*/