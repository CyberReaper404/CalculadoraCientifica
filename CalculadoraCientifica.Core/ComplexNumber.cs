namespace CalculadoraCientifica.Core;

public readonly record struct ComplexNumber(double Real, double Imaginary)
{
    public double Modulus => Math.Sqrt((Real * Real) + (Imaginary * Imaginary));

    public ComplexNumber Add(ComplexNumber other) =>
        new(Real + other.Real, Imaginary + other.Imaginary);

    public ComplexNumber Multiply(ComplexNumber other) =>
        new(
            (Real * other.Real) - (Imaginary * other.Imaginary),
            (Real * other.Imaginary) + (Imaginary * other.Real));
}
