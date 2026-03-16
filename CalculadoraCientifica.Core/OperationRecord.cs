namespace CalculadoraCientifica.Core;

public sealed record OperationRecord(
    string Category,
    string Expression,
    string Result,
    DateTime Timestamp)
{
    public string TimestampLabel => Timestamp.ToString("HH:mm:ss");
}
