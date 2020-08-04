using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace DurableEntitiesTemplate
{
    [FunctionName("CounterOrchestration")]
    public static async Task Run(
    [OrchestrationTrigger] IDurableOrchestrationContext context)
    {
        var entityId = new EntityId(nameof(Counter), "myCounter");

        // Two-way call to the entity which returns a value - awaits the response
        int currentValue = await context.CallEntityAsync<int>(entityId, "Get");
        if (currentValue < 10)
        {
            // One-way signal to the entity which updates the value - does not await a response
            context.SignalEntity(entityId, "Add", 1);
        }
    }
}