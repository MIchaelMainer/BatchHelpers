// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------

/**
Option 3: Batch singleton request builder

Use the existing request builders to build up the request URL, query options, and request headers.

**/

// Check that we don't exceed the maximum number of batch objects. We'd implement a check and
// ClientException thrown in addToBatch() if exceeded.
// The singleton Batch object would need to be instantiated with the GraphServiceClient.
// Only one batch could be active at a time per GraphServiceClient object. 
if (graphClient.batch.spaceRemaining() >= 2){
      //specify request, verb, custom headers, body if exists (overloads), batch id, depends on (id)
      graphClient.Me.Drive.Request(Options).addToBatch ("get", "1"); 
      // request 2 depends on request 1
      graphClient.Groups.Request(Options).addToBatch("post", groupBody, "2", ["1"]);
}

// Send the batch with header and query param options. 
BatchResponse res = await graphClient.batch.Request(Options).PostAsync();
HttpHeaders headers = res.Headers;
HttpStatusCode statusCode = res.StatusCode;
JsonObject rawBody = res.RawBody.ToJsonObject();

Collection<BatchResponsePart> parts = res.BatchResponseParts;
Serializer serializer = new Serializer();
foreach (BatchResponsePart part in parts)
{
      // Dev would need to track the part id and map which call was made to 
      // determine the expected body entity. They'd maybe use a dictionary to
      // track this information.
      if (part.Id == "1")
      {
            HttpHeaders partHeaders = part.Headers;
            HttpStatusCode partStatusCode = part.StatusCode;
            int id = part.Id;
            JsonObject body = part.Body;
            Drive drive = serializer.Deserializer<Drive>(body);
      }
}
/* Open questions for Option 3.
What happens if one of the batch response parts is a redirect?
What is the experience if one of the requests requires a scope that is not granted? This would be like any other expected Graph error scenario.
How do we really expect customers to map the request id to response batch part id, and then know what is the expected response entity? 
*/