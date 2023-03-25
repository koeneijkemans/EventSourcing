using Azure;
using Azure.Data.Tables;

namespace Kei.EventSourcing.Store.AzureTable;

public class TableEvent : ITableEntity, IEventStoreItem
{
    public Guid AggregateId { get; set; }

    public int Order { get; set; }

    public string? Data { get; set; }

    public string? EventType { get; set; }

    public string? PartitionKey { get; set; }

    public string? RowKey { get; set; }

    public DateTimeOffset? Timestamp { get; set; }

    public ETag ETag { get; set; }
}
