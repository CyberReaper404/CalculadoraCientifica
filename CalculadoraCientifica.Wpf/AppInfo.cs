using System.Reflection;

namespace CalculadoraCientifica.Wpf;

public static class AppInfo
{
    public static string ProductName => "MM CALC Científica";

    public static string Version =>
        Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "1.0.0";
}
