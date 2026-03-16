using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CalculadoraCientifica.Core;

namespace CalculadoraCientifica.Wpf;

public partial class MainWindow : Window
{
    private const string BasicCategory = "Operações básicas";
    private const string ScientificCategory = "Trigonometria";
    private const string LogarithmCategory = "Logaritmos";
    private const string PowerCategory = "Potência e raízes";
    private const string ComplexCategory = "Números complexos";
    private readonly ObservableCollection<OperationRecord> _history = [];
    private readonly AppStateService _appStateService = new();
    private readonly TextBox[] _inputFields;
    private const double DisplayZeroTolerance = 1e-12;
    private static readonly NumberStyles ParseNumberStyles = NumberStyles.Float | NumberStyles.AllowThousands;
    private static readonly CultureInfo[] ParseCultures =
    [
        CultureInfo.CurrentCulture,
        CultureInfo.InvariantCulture,
        new CultureInfo("pt-BR"),
        new CultureInfo("en-US")
    ];

    public MainWindow()
    {
        InitializeComponent();
        Title = AppInfo.ProductName;
        LoadWindowIcon();
        _inputFields =
        [
            BasicFirstInput,
            BasicSecondInput,
            PowerBaseInput,
            PowerExponentInput,
            RootInput,
            TrigInput,
            LogInput,
            LogBaseInput,
            ComplexFirstRealInput,
            ComplexFirstImaginaryInput,
            ComplexSecondRealInput,
            ComplexSecondImaginaryInput
        ];
        HistoryList.ItemsSource = _history;
        VersionText.Text = $"Versão {AppInfo.Version}";
        RestoreAppState();
    }

    private void LoadWindowIcon()
    {
        Icon = BitmapFrame.Create(new Uri("pack://application:,,,/Assets/mm-calc-icon.png", UriKind.Absolute));
    }

    private void RestoreAppState()
    {
        var state = _appStateService.Load();
        AngleUnitCombo.SelectedIndex = Math.Clamp(state.SelectedAngleUnitIndex, 0, 1);

        foreach (var item in state.History.Take(50))
        {
            _history.Add(item);
        }

        if (_history.Count > 0)
        {
            LastExpressionText.Text = _history[0].Expression;
            CurrentResultText.Text = _history[0].Result;
            StatusText.Text = "Sessão anterior restaurada.";
        }
    }

    private void PersistAppState()
    {
        _appStateService.Save(AngleUnitCombo.SelectedIndex, _history);
    }

    private void BasicOperationClick(object sender, RoutedEventArgs e)
    {
        RunOperation(() =>
        {
            var left = ParseRequiredDouble(BasicFirstInput.Text, "Primeiro número");
            var right = ParseRequiredDouble(BasicSecondInput.Text, "Segundo número");

            return GetButtonTag(sender) switch
            {
                "Add" => CreateNumericResult(BasicCategory, $"{FormatNumber(left)} + {FormatNumber(right)}", ScientificCalculator.Add(left, right)),
                "Subtract" => CreateNumericResult(BasicCategory, $"{FormatNumber(left)} - {FormatNumber(right)}", ScientificCalculator.Subtract(left, right)),
                "Multiply" => CreateNumericResult(BasicCategory, $"{FormatNumber(left)} x {FormatNumber(right)}", ScientificCalculator.Multiply(left, right)),
                "Divide" => CreateNumericResult(BasicCategory, $"{FormatNumber(left)} / {FormatNumber(right)}", ScientificCalculator.Divide(left, right)),
                _ => throw new CalculationException("Operação básica desconhecida.")
            };
        });
    }

    private void PowerOperationClick(object sender, RoutedEventArgs e)
    {
        RunOperation(() =>
        {
            return GetButtonTag(sender) switch
            {
                "Power" => BuildPowerResult(),
                "SquareRoot" => BuildSquareRootResult(),
                "CubeRoot" => BuildCubeRootResult(),
                _ => throw new CalculationException("Operação de potência ou raiz desconhecida.")
            };
        });
    }

    private void TrigonometryOperationClick(object sender, RoutedEventArgs e)
    {
        RunOperation(() =>
        {
            var unit = GetAngleUnit();
            var input = ParseRequiredDouble(TrigInput.Text, "Entrada trigonométrica");
            var label = unit == AngleUnit.Degrees ? "graus" : "rad";

            return GetButtonTag(sender) switch
            {
                "Sine" => CreateNumericResult(ScientificCategory, $"sen({FormatNumber(input)} {label})", ScientificCalculator.Sine(input, unit)),
                "Cosine" => CreateNumericResult(ScientificCategory, $"cos({FormatNumber(input)} {label})", ScientificCalculator.Cosine(input, unit)),
                "Tangent" => CreateNumericResult(ScientificCategory, $"tan({FormatNumber(input)} {label})", ScientificCalculator.Tangent(input, unit)),
                "ArcSine" => CreateResult(ScientificCategory, $"arcsen({FormatNumber(input)})", FormatAngle(ScientificCalculator.ArcSine(input, unit), unit)),
                "ArcCosine" => CreateResult(ScientificCategory, $"arccos({FormatNumber(input)})", FormatAngle(ScientificCalculator.ArcCosine(input, unit), unit)),
                "ArcTangent" => CreateResult(ScientificCategory, $"arctan({FormatNumber(input)})", FormatAngle(ScientificCalculator.ArcTangent(input, unit), unit)),
                _ => throw new CalculationException("Operação trigonométrica desconhecida.")
            };
        });
    }

    private void LogarithmOperationClick(object sender, RoutedEventArgs e)
    {
        RunOperation(() =>
        {
            var value = ParseRequiredDouble(LogInput.Text, "Número");

            return GetButtonTag(sender) switch
            {
                "NaturalLog" => CreateNumericResult(LogarithmCategory, $"ln({FormatNumber(value)})", ScientificCalculator.NaturalLog(value)),
                "Base10Log" => CreateNumericResult(LogarithmCategory, $"log10({FormatNumber(value)})", ScientificCalculator.Base10Log(value)),
                "CustomLog" => BuildCustomLogResult(value),
                _ => throw new CalculationException("Operação de logaritmo desconhecida.")
            };
        });
    }

    private void ComplexOperationClick(object sender, RoutedEventArgs e)
    {
        RunOperation(() =>
        {
            var first = ParseComplex(ComplexFirstRealInput.Text, ComplexFirstImaginaryInput.Text, "z1");

            return GetButtonTag(sender) switch
            {
                "AddComplex" => BuildComplexAdditionResult(first),
                "MultiplyComplex" => BuildComplexMultiplicationResult(first),
                "Modulus" => CreateNumericResult(ComplexCategory, $"|{FormatComplex(first)}|", ScientificCalculator.ComplexModulus(first)),
                "Polar" => CreateResult(ComplexCategory, $"polar({FormatComplex(first)})", FormatPolar(ScientificCalculator.ToPolar(first, GetAngleUnit()))),
                _ => throw new CalculationException("Operação complexa desconhecida.")
            };
        });
    }

    private void AngleUnitCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (StatusText is null)
        {
            return;
        }

        StatusText.Text = GetAngleUnit() == AngleUnit.Degrees
            ? "Modo angular ativo: graus."
            : "Modo angular ativo: radianos.";

        PersistAppState();
    }

    private void ClearInputsClick(object sender, RoutedEventArgs e)
    {
        foreach (var textBox in _inputFields)
        {
            textBox.Clear();
        }

        CurrentResultText.Text = "0";
        LastExpressionText.Text = "Entradas limpas";
        StatusText.Text = "Os campos foram limpos. Histórico mantido.";
    }

    private void ClearHistoryClick(object sender, RoutedEventArgs e)
    {
        _history.Clear();
        LastExpressionText.Text = "Histórico limpo";
        CurrentResultText.Text = "0";
        StatusText.Text = "As operações anteriores foram removidas da sessão.";
        PersistAppState();
    }

    private void CopyResultClick(object sender, RoutedEventArgs e)
    {
        Clipboard.SetText(CurrentResultText.Text);
        StatusText.Text = "Resultado copiado para a área de transferência.";
    }

    private void AboutClick(object sender, RoutedEventArgs e)
    {
        MessageBox.Show(
            $"{AppInfo.ProductName}\nVersão {AppInfo.Version}\n\nCalculadora científica desktop com testes automatizados, histórico persistido e empacotamento para distribuição.",
            "Sobre o MM CALC Científica",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private DisplayResult BuildPowerResult()
    {
        var baseValue = ParseRequiredDouble(PowerBaseInput.Text, "Base");
        var exponent = ParseRequiredDouble(PowerExponentInput.Text, "Expoente");

        return CreateNumericResult(
            PowerCategory,
            $"{FormatNumber(baseValue)}^{FormatNumber(exponent)}",
            ScientificCalculator.Power(baseValue, exponent));
    }

    private DisplayResult BuildSquareRootResult()
    {
        var value = ParseRequiredDouble(RootInput.Text, "Valor para raiz");
        return CreateNumericResult(PowerCategory, $"sqrt({FormatNumber(value)})", ScientificCalculator.SquareRoot(value));
    }

    private DisplayResult BuildCubeRootResult()
    {
        var value = ParseRequiredDouble(RootInput.Text, "Valor para raiz");
        return CreateNumericResult(PowerCategory, $"cbrt({FormatNumber(value)})", ScientificCalculator.CubeRoot(value));
    }

    private DisplayResult BuildCustomLogResult(double value)
    {
        var baseValue = ParseRequiredDouble(LogBaseInput.Text, "Base personalizada");
        return CreateNumericResult(
            LogarithmCategory,
            $"log na base {FormatNumber(baseValue)} de {FormatNumber(value)}",
            ScientificCalculator.Log(value, baseValue));
    }

    private DisplayResult BuildComplexAdditionResult(ComplexNumber first)
    {
        var second = ParseComplex(ComplexSecondRealInput.Text, ComplexSecondImaginaryInput.Text, "z2");
        return CreateResult(
            ComplexCategory,
            $"{FormatComplex(first)} + {FormatComplex(second)}",
            FormatComplex(ScientificCalculator.AddComplex(first, second)));
    }

    private DisplayResult BuildComplexMultiplicationResult(ComplexNumber first)
    {
        var second = ParseComplex(ComplexSecondRealInput.Text, ComplexSecondImaginaryInput.Text, "z2");
        return CreateResult(
            ComplexCategory,
            $"{FormatComplex(first)} x {FormatComplex(second)}",
            FormatComplex(ScientificCalculator.MultiplyComplex(first, second)));
    }

    private void RunOperation(Func<DisplayResult> operation)
    {
        try
        {
            RegisterResult(operation());
        }
        catch (CalculationException ex)
        {
            ShowError(ex.Message);
        }
    }

    private void RegisterResult(DisplayResult result)
    {
        LastExpressionText.Text = result.Expression;
        CurrentResultText.Text = result.Result;
        StatusText.Text = $"{result.Category} executada com sucesso.";
        _history.Insert(0, new OperationRecord(result.Category, result.Expression, result.Result, DateTime.Now));
        PersistAppState();
    }

    private void ShowError(string message)
    {
        LastExpressionText.Text = "Não foi possível concluir a operação";
        CurrentResultText.Text = "Erro";
        StatusText.Text = message;
    }

    private static DisplayResult CreateNumericResult(string category, string expression, double value) =>
        new(category, expression, FormatNumber(value));

    private static DisplayResult CreateResult(string category, string expression, string result) =>
        new(category, expression, result);

    private static ComplexNumber ParseComplex(string realText, string imaginaryText, string name) =>
        new(
            ParseRequiredDouble(realText, $"{name} parte real"),
            ParseRequiredDouble(imaginaryText, $"{name} parte imaginária"));

    private static double ParseRequiredDouble(string? rawValue, string label)
    {
        if (TryParseFlexibleDouble(rawValue, out var value))
        {
            return value;
        }

        throw new CalculationException($"{label} inválido. Use um número válido.");
    }

    private static bool TryParseFlexibleDouble(string? rawValue, out double value)
    {
        value = 0;

        if (string.IsNullOrWhiteSpace(rawValue))
        {
            return false;
        }

        foreach (var culture in ParseCultures)
        {
            if (double.TryParse(rawValue, ParseNumberStyles, culture, out value))
            {
                return true;
            }
        }

        var normalized = rawValue.Trim().Replace(" ", string.Empty);

        if (normalized.Contains(',') && normalized.Contains('.'))
        {
            normalized = normalized.LastIndexOf(',') > normalized.LastIndexOf('.')
                ? normalized.Replace(".", string.Empty).Replace(',', '.')
                : normalized.Replace(",", string.Empty);
        }
        else
        {
            normalized = normalized.Replace(',', '.');
        }

        return double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
    }

    private AngleUnit GetAngleUnit() =>
        AngleUnitCombo.SelectedIndex == 1 ? AngleUnit.Radians : AngleUnit.Degrees;

    private static string GetButtonTag(object sender) =>
        sender is Button button && button.Tag is string tag
            ? tag
            : throw new CalculationException("Não foi possível identificar a operação.");

    private static string FormatNumber(double value)
    {
        if (Math.Abs(value) < DisplayZeroTolerance)
        {
            value = 0;
        }

        return value.ToString("0.###############", CultureInfo.CurrentCulture);
    }

    private static string FormatAngle(double value, AngleUnit unit) =>
        unit == AngleUnit.Degrees
            ? $"{FormatNumber(value)} graus"
            : $"{FormatNumber(value)} rad";

    private static string FormatComplex(ComplexNumber value)
    {
        var sign = value.Imaginary < 0 ? "-" : "+";
        return $"{FormatNumber(value.Real)} {sign} {FormatNumber(Math.Abs(value.Imaginary))}i";
    }

    private static string FormatPolar(PolarForm value)
    {
        var phase = value.Unit == AngleUnit.Degrees
            ? $"{FormatNumber(value.Phase)} graus"
            : $"{FormatNumber(value.Phase)} rad";

        return $"módulo {FormatNumber(value.Magnitude)}, fase {phase}";
    }

    private sealed record DisplayResult(string Category, string Expression, string Result);
}
