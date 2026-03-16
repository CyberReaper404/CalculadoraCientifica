using System.IO;
using System.Text.Json;
using CalculadoraCientifica.Core;

namespace CalculadoraCientifica.Wpf;

public sealed class AppStateService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    private readonly string _stateFilePath;

    public AppStateService()
    {
        var overrideDirectory = Environment.GetEnvironmentVariable("MM_CALC_STATE_DIR");
        var appDataDirectory = string.IsNullOrWhiteSpace(overrideDirectory)
            ? Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "MMCalcCientifica")
            : overrideDirectory;

        Directory.CreateDirectory(appDataDirectory);
        _stateFilePath = Path.Combine(appDataDirectory, "app-state.json");
    }

    public AppState Load()
    {
        if (!File.Exists(_stateFilePath))
        {
            return new AppState();
        }

        try
        {
            var json = File.ReadAllText(_stateFilePath);
            return JsonSerializer.Deserialize<AppState>(json, JsonOptions) ?? new AppState();
        }
        catch
        {
            return new AppState();
        }
    }

    public void Save(int selectedAngleUnitIndex, IEnumerable<OperationRecord> history)
    {
        var state = new AppState
        {
            SelectedAngleUnitIndex = selectedAngleUnitIndex,
            History = history.Take(50).ToList()
        };

        try
        {
            var json = JsonSerializer.Serialize(state, JsonOptions);
            File.WriteAllText(_stateFilePath, json);
        }
        catch
        {
            // Ignore persistence failures so the calculator can keep working.
        }
    }
}
