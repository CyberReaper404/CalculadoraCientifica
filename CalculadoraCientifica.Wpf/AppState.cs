using CalculadoraCientifica.Core;

namespace CalculadoraCientifica.Wpf;

public sealed class AppState
{
    public int SelectedAngleUnitIndex { get; init; }

    public List<OperationRecord> History { get; init; } = [];
}
