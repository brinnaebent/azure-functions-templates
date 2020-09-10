﻿﻿import { AzureFunction, Context, HttpRequest } from "@azure/functions"
import * as df from "durable-functions"

const httpStart: AzureFunction = async function (context: Context, req: HttpRequest) {
    const client = df.getClient(context);
    const id: string = context.bindingData.id;
    const entityId = new df.EntityId("Counter", id);

    if (req.method === "POST") {
        // increment value
        await client.signalEntity(entityId, "add", 1);
    } else {
        // reads current state of entity
        return await client.readEntityState<number>(entityId);
    }
};

export default httpStart;