namespace Kei.EventSourcing.Store.AzureTable;

public class AzureTableEventStoreOptions
{
    public string? ConnectionString { get; set; }

    public string? TableName { get; set; }
}
