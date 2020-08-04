using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace DurableEntitiesTemplate
{
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