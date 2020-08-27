using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class DurableFunctionsEntityOrchestrationCSharp
    {
        [FunctionName("DurableFunctionsEntityOrchestrationCSharp_CounterHttpStart")]
        public static async Task<HttpResponseMessage> Run(
        [HttpTrigger(AuthorizationLevel.Function, Route = "Counter/{entityKey}")] HttpRequestMessage req,
        [DurableClient] IDurableEntityClient client,
        string entityKey)
        {
            var entityId = new EntityId("Counter", entityKey);
            await client.SignalEntity(entityId, "add", 1);
            return client.CreateCheckStatusResponse(req, instanceId);
        }

        [FunctionName("Counter")]
        public static void Counter([EntityTrigger] IDurableEntityContext ctx)
        {
            switch (ctx.OperationName.ToLowerInvariant())
            {
                case "add":
                    ctx.SetState(ctx.GetState<int>() + ctx.GetInput<int>());
                    break;
                case "reset":
                    ctx.SetState(0);
                    break;
                case "get":
                    ctx.Return(ctx.GetState<int>());
                    break;
            }
        }
    }
}