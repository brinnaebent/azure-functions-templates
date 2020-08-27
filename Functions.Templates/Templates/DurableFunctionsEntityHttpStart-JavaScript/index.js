const df = require("durable-functions");

module.exports = async function (context, req) {
    const client = df.getClient(context);
    const entityId = new df.EntityId("Counter", req.params.id);

    await client.signalEntity(entityId, "add", 1);
};