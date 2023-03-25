using Azure;
using Azure.Data.Tables;
using Kei.EventSourcing.Interfaces;
using Newtonsoft.Json;

namespace Kei.EventSourcing.Store.AzureTable;

public class AzureTableEventStore : EventStore
{
    private TableClient _tableClient;

    public AzureTableEventStore(IEventPublisher eventPublisher, AzureTableEventStoreOptions options)
        : base(eventPublisher)
    {
        _tableClient = new TableClient(options.ConnectionString, options.TableName);
    }

    public override IEnumerable<Event> Get(Guid aggregateId)
    {
        List<Event> allEvents = new();
        Pageable<TableEvent> result = _tableClient.Query<TableEvent>(e => e.PartitionKey == aggregateId.ToString());

        foreach (TableEvent tableEvent in result)
        {
            allEvents.Add(ToEvent(tableEvent));
        }

        return allEvents
            .OrderBy(e => e.Order)
            .ToList();
    }

    protected override void SaveInStore(Event @event)
    {
        TableEvent tableEvent = new()
        {
            AggregateId = @event.AggregateRootId,
            Order = @event.Order,
            PartitionKey = @event.AggregateRootId.ToString(),
            RowKey = @event.Order.ToString(),
            EventType = @event.GetType().Name,
            Data = JsonConvert.SerializeObject(@event),
            Timestamp = DateTime.UtcNow,
            ETag = ETag.All,
        };

        _tableClient.AddEntity(tableEvent);
    }
}
