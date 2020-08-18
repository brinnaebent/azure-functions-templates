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
        [FunctionName("DurableFunctionsEntityOrchestrationCSharp_CounterOrchestrator")]
        public static async Task Orchestrator(
        [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var entityId = new EntityId(nameof(Counter), "myCounter");

            // Two-way call to the entity which returns a value - awaits the response
            int currentValue = await context.CallEntityAsync<int>(entityId, "get");
            if (currentValue < 10)
            {
                // One-way signal to the entity which updates the value - does not await a response
                context.SignalEntity(entityId, "add", 1);
            }
        }

        [FunctionName("DurableFunctionsEntityOrchestrationCSharp_CounterHttpStart")]
        public static async Task<HttpResponseMessage> Run(
        [HttpTrigger(AuthorizationLevel.Function)] HttpRequestMessage req,
        [DurableClient] IDurableOrchestrationClient client,
        ILogger log)
        {
            string instanceId = await client.StartNewAsync("DurableFunctionsEntityOrchestrationCSharp_CounterOrchestrator", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

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