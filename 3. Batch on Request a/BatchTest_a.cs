var contact = new Contact();
contact.GivenName = "_Tom" + Guid.NewGuid().ToString();

RequestBatchPart<Contact> postNewContactBatchPart = graphClient.Me.Contacts.Request().BatchPartAdd(contact);
RequestBatchPart<User> getUserBatchPart = graphClient.Me.Request().BatchPartGet();

List<RequestBatchPart<Contact>> contactContactBatchParts = new List<RequestBatchPart<Contact>>();
contactContactBatchParts.Add(postNewContactBatchPart);
List<IBatchPart> batchParts = new List<IBatchPart>(contactContactBatchParts);
batchParts.Add(getUserBatchPart);

BatchRequest batchRequest = new BatchRequest(batchParts);

BatchResponse batchResponse;
try
{
    batchResponse = await graphClient.Batch.PostBatchAsync(batchRequest);
    // At this point, you can get the overall response status and headers.
    // You also can get the raw response. This will be used to deserialize
    // the response bodies. You can also implement a custom deserializer.
}
catch (ServiceException)
{
    throw;
}

// Get information about the results of the BatchPart.
postNewContactBatchPart.LoadBatchPartResponse(batchResponse.ResponseBody);
Contact myContact = postNewContactBatchPart.ResponseBody;
var contactBatchPartResponseHeaders = postNewContactBatchPart.ResponseHeaders;
var contactBatchPartResponseHttpStatusCode = postNewContactBatchPart.ResponseHttpStatusCode;

getUserBatchPart.LoadBatchPartResponse(batchResponse.ResponseBody);
User myUser = getUserBatchPart.ResponseBody;
var userBatchPartResponseHeaders = getUserBatchPart.ResponseHeaders;
var userBatchPartResponseHttpStatusCode = getUserBatchPart.ResponseHttpStatusCode;