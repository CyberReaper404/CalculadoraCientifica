namespace CalculadoraCientifica.Core;

public static class ScientificCalculator
{
    private const double ZeroTolerance = 1e-10;

    public static double Add(double left, double right)
    {
        EnsureFinite(left, "O primeiro número deve ser finito.");
        EnsureFinite(right, "O segundo número deve ser finito.");
        return left + right;
    }

    public static double Subtract(double left, double right)
    {
        EnsureFinite(left, "O primeiro número deve ser finito.");
        EnsureFinite(right, "O segundo número deve ser finito.");
        return left - right;
    }

    public static double Multiply(double left, double right)
    {
        EnsureFinite(left, "O primeiro número deve ser finito.");
        EnsureFinite(right, "O segundo número deve ser finito.");
        return left * right;
    }

    public static double Divide(double left, double right)
    {
        EnsureFinite(left, "O dividendo deve ser finito.");
        EnsureFinite(right, "O divisor deve ser finito.");
        EnsureNotZero(right, "Divisão por zero não é permitida.");
        return left / right;
    }

    public static double Power(double baseValue, double exponent)
    {
        EnsureFinite(baseValue, "A base deve ser finita.");
        EnsureFinite(exponent, "O expoente deve ser finito.");
        return Math.Pow(baseValue, exponent);
    }

    public static double SquareRoot(double value)
    {
        EnsureFinite(value, "O valor deve ser finito.");

        if (value < 0)
        {
            throw new CalculationException("Raiz quadrada de número negativo não possui resultado real.");
        }

        return Math.Sqrt(value);
    }

    public static double CubeRoot(double value)
    {
        EnsureFinite(value, "O valor deve ser finito.");
        return Math.Cbrt(value);
    }

    public static double Sine(double value, AngleUnit unit = AngleUnit.Degrees)
    {
        EnsureFinite(value, "O valor trigonométrico deve ser finito.");
        return Math.Sin(ToRadians(value, unit));
    }

    public static double Cosine(double value, AngleUnit unit = AngleUnit.Degrees)
    {
        EnsureFinite(value, "O valor trigonométrico deve ser finito.");
        return Math.Cos(ToRadians(value, unit));
    }

    public static double Tangent(double value, AngleUnit unit = AngleUnit.Degrees)
    {
        EnsureFinite(value, "O valor trigonométrico deve ser finito.");
        var radians = ToRadians(value, unit);
        var cosine = Math.Cos(radians);

        if (Math.Abs(cosine) < ZeroTolerance)
        {
            throw new CalculationException("Tangente indefinida para esse ângulo.");
        }

        return Math.Tan(radians);
    }

    public static double ArcSine(double value, AngleUnit unit = AngleUnit.Degrees)
    {
        EnsureFinite(value, "O valor trigonométrico deve ser finito.");
        EnsureInsideUnitInterval(value, "Arco seno aceita apenas valores entre -1 e 1.");
        return FromRadians(Math.Asin(value), unit);
    }

    public static double ArcCosine(double value, AngleUnit unit = AngleUnit.Degrees)
    {
        EnsureFinite(value, "O valor trigonométrico deve ser finito.");
        EnsureInsideUnitInterval(value, "Arco cosseno aceita apenas valores entre -1 e 1.");
        return FromRadians(Math.Acos(value), unit);
    }

    public static double ArcTangent(double value, AngleUnit unit = AngleUnit.Degrees)
    {
        EnsureFinite(value, "O valor trigonométrico deve ser finito.");
        return FromRadians(Math.Atan(value), unit);
    }

    public static double NaturalLog(double value)
    {
        EnsureFinite(value, "O valor do logaritmo deve ser finito.");
        EnsurePositive(value, "Logaritmos exigem números maiores que zero.");
        return Math.Log(value);
    }

    public static double Base10Log(double value)
    {
        EnsureFinite(value, "O valor do logaritmo deve ser finito.");
        EnsurePositive(value, "Logaritmos exigem números maiores que zero.");
        return Math.Log10(value);
    }

    public static double Log(double value, double baseValue)
    {
        EnsureFinite(value, "O valor do logaritmo deve ser finito.");
        EnsureFinite(baseValue, "A base do logaritmo deve ser finita.");
        EnsurePositive(value, "Logaritmos exigem números maiores que zero.");
        EnsurePositive(baseValue, "A base do logaritmo deve ser maior que zero e diferente de 1.");

        if (Math.Abs(baseValue - 1d) < ZeroTolerance)
        {
            throw new CalculationException("A base do logaritmo deve ser maior que zero e diferente de 1.");
        }

        return Math.Log(value, baseValue);
    }

    public static ComplexNumber AddComplex(ComplexNumber left, ComplexNumber right) =>
        EnsureFinite(left, "z1 deve conter valores finitos.")
            .Add(EnsureFinite(right, "z2 deve conter valores finitos."));

    public static ComplexNumber MultiplyComplex(ComplexNumber left, ComplexNumber right) =>
        EnsureFinite(left, "z1 deve conter valores finitos.")
            .Multiply(EnsureFinite(right, "z2 deve conter valores finitos."));

    public static double ComplexModulus(ComplexNumber value) => EnsureFinite(value, "O número complexo deve conter valores finitos.").Modulus;

    public static PolarForm ToPolar(ComplexNumber value, AngleUnit unit = AngleUnit.Degrees)
    {
        EnsureFinite(value, "O número complexo deve conter valores finitos.");
        var phaseInRadians = Math.Atan2(value.Imaginary, value.Real);
        return new PolarForm(value.Modulus, FromRadians(phaseInRadians, unit), unit);
    }

    private static double ToRadians(double value, AngleUnit unit) =>
        unit == AngleUnit.Degrees ? value * (Math.PI / 180d) : value;

    private static double FromRadians(double value, AngleUnit unit) =>
        unit == AngleUnit.Degrees ? value * (180d / Math.PI) : value;

    private static void EnsureNotZero(double value, string message)
    {
        if (Math.Abs(value) < ZeroTolerance)
        {
            throw new CalculationException(message);
        }
    }

    private static void EnsurePositive(double value, string message)
    {
        if (value <= 0)
        {
            throw new CalculationException(message);
        }
    }

    private static void EnsureInsideUnitInterval(double value, string message)
    {
        if (value < -1 || value > 1)
        {
            throw new CalculationException(message);
        }
    }

    private static void EnsureFinite(double value, string message)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
        {
            throw new CalculationException(message);
        }
    }

    private static ComplexNumber EnsureFinite(ComplexNumber value, string message)
    {
        EnsureFinite(value.Real, message);
        EnsureFinite(value.Imaginary, message);
        return value;
    }
}
