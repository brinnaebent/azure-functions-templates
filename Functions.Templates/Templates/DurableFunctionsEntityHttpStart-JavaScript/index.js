const df = require("durable-functions");

module.exports = async function (context) {
    const client = df.getClient(context);
    const entityId = new df.EntityId("Counter", "myCounter");

    await client.signalEntity(entityId, "add", 1);

    const stateResponse = await client.readEntityState(entityId);
    return stateResponse.entityState;
};