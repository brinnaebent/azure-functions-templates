using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace DurableEntitiesTemplate
{
    [FunctionName("CounterHttpStarter")]
    public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter)
    {
        var entityId = new EntityId(nameof(Counter), "myCounter");
        EntityStateResponse<JObject> stateResponse = await client.ReadEntityStateAsync<JObject>(entityId);
        return req.CreateResponse(HttpStatusCode.OK, stateResponse.EntityState);
    }
}