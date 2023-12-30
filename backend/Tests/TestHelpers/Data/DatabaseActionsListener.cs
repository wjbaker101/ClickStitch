using Data.Types;

namespace TestHelpers.Data;

public sealed class DatabaseActionsListener
{
    public List<IDatabaseRecord> Saved { get; } = new();
    public List<IDatabaseRecord> Updated { get; } = new();
    public List<IDatabaseRecord> Delete { get; } = new();

    public void OnSave<TRecord>(TRecord record) where TRecord : IDatabaseRecord
    {
        Saved.Add(record);
    }

    public void OnUpdate<TRecord>(TRecord record) where TRecord : IDatabaseRecord
    {
        Updated.Add(record);
    }

    public void OnDelete<TRecord>(TRecord record) where TRecord : IDatabaseRecord
    {
        Delete.Add(record);
    }
}