#r "Microsoft.Azure.WebJobs.Extensions.DurableTask"
#r "Newtonsoft.Json"

using System.Net;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

public static async Task<HttpResponseMessage> Run(
    [HttpTrigger(AuthorizationLevel.Function, Route = "Counter/{entityKey}")] HttpRequestMessage req,
    [DurableClient] IDurableEntityClient client,
    string entityKey)
{
    var entityId = new EntityId("Counter", entityKey);

    if (req.Method == HttpMethod.Post)
    {
        await client.SignalEntityAsync(entityId, "add", 1);
        return req.CreateResponse(HttpStatusCode.OK);
    }

    EntityStateResponse<JObject> stateResponse = await client.ReadEntityStateAsync<JObject>(entityId);
    return req.CreateResponse(HttpStatusCode.OK, stateResponse.EntityState);
}