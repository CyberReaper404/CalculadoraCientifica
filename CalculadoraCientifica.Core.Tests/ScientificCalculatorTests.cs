using CalculadoraCientifica.Core;

namespace CalculadoraCientifica.Core.Tests;

[TestClass]
public sealed class ScientificCalculatorTests
{
    [TestMethod]
    public void Add_ShouldReturnExpectedValue()
    {
        Assert.AreEqual(15d, ScientificCalculator.Add(10, 5), 1e-10);
    }

    [TestMethod]
    public void Add_ShouldHandleNegativeNumbers()
    {
        Assert.AreEqual(-3d, ScientificCalculator.Add(-7, 4), 1e-10);
    }

    [TestMethod]
    public void Subtract_ShouldReturnExpectedValue()
    {
        Assert.AreEqual(5d, ScientificCalculator.Subtract(10, 5), 1e-10);
    }

    [TestMethod]
    public void Subtract_ShouldHandleNegativeResult()
    {
        Assert.AreEqual(-12d, ScientificCalculator.Subtract(-5, 7), 1e-10);
    }

    [TestMethod]
    public void Multiply_ShouldReturnExpectedValue()
    {
        Assert.AreEqual(50d, ScientificCalculator.Multiply(10, 5), 1e-10);
    }

    [TestMethod]
    public void Multiply_ShouldHandleZero()
    {
        Assert.AreEqual(0d, ScientificCalculator.Multiply(999, 0), 1e-10);
    }

    [TestMethod]
    public void Divide_ShouldReturnExpectedValue()
    {
        Assert.AreEqual(2d, ScientificCalculator.Divide(10, 5), 1e-10);
    }

    [TestMethod]
    public void Divide_ShouldHandleNegativeQuotient()
    {
        Assert.AreEqual(-4d, ScientificCalculator.Divide(12, -3), 1e-10);
    }

    [TestMethod]
    public void Divide_ShouldThrow_WhenDivisorIsZero()
    {
        Assert.ThrowsExactly<CalculationException>(() => ScientificCalculator.Divide(10, 0));
    }

    [TestMethod]
    public void Divide_ShouldThrow_WhenDivisorIsVeryCloseToZero()
    {
        Assert.ThrowsExactly<CalculationException>(() => ScientificCalculator.Divide(10, 1e-12));
    }

    [TestMethod]
    public void Divide_ShouldAllowBoundaryToleranceValue()
    {
        Assert.AreEqual(1e10d, ScientificCalculator.Divide(1, 1e-10), 1e-2);
    }

    [TestMethod]
    public void Power_ShouldReturnExpectedValue()
    {
        Assert.AreEqual(81d, ScientificCalculator.Power(3, 4), 1e-10);
    }

    [TestMethod]
    public void Power_ShouldSupportFractionalExponent()
    {
        Assert.AreEqual(3d, ScientificCalculator.Power(9, 0.5d), 1e-10);
    }

    [TestMethod]
    public void SquareRoot_ShouldReturnExpectedValue()
    {
        Assert.AreEqual(9d, ScientificCalculator.SquareRoot(81), 1e-10);
    }

    [TestMethod]
    public void SquareRoot_ShouldHandleZero()
    {
        Assert.AreEqual(0d, ScientificCalculator.SquareRoot(0), 1e-10);
    }

    [TestMethod]
    public void SquareRoot_ShouldThrow_WhenInputIsNegative()
    {
        Assert.ThrowsExactly<CalculationException>(() => ScientificCalculator.SquareRoot(-1));
    }

    [TestMethod]
    public void SquareRoot_ShouldRejectInfinity()
    {
        Assert.ThrowsExactly<CalculationException>(() => ScientificCalculator.SquareRoot(double.PositiveInfinity));
    }

    [TestMethod]
    public void CubeRoot_ShouldHandleNegativeValue()
    {
        Assert.AreEqual(-4d, ScientificCalculator.CubeRoot(-64), 1e-10);
    }

    [TestMethod]
    public void CubeRoot_ShouldHandleZero()
    {
        Assert.AreEqual(0d, ScientificCalculator.CubeRoot(0), 1e-10);
    }

    [TestMethod]
    public void CubeRoot_ShouldRejectNaN()
    {
        Assert.ThrowsExactly<CalculationException>(() => ScientificCalculator.CubeRoot(double.NaN));
    }

    [TestMethod]
    public void Sine_ShouldRespectDegrees()
    {
        Assert.AreEqual(0.5d, ScientificCalculator.Sine(30, AngleUnit.Degrees), 1e-10);
    }

    [TestMethod]
    public void Sine_ShouldRespectRadians()
    {
        Assert.AreEqual(1d, ScientificCalculator.Sine(Math.PI / 2d, AngleUnit.Radians), 1e-10);
    }

    [TestMethod]
    public void Cosine_ShouldRespectDegrees()
    {
        Assert.AreEqual(0.5d, ScientificCalculator.Cosine(60, AngleUnit.Degrees), 1e-10);
    }

    [TestMethod]
    public void Cosine_ShouldRespectRadians()
    {
        Assert.AreEqual(-1d, ScientificCalculator.Cosine(Math.PI, AngleUnit.Radians), 1e-10);
    }

    [TestMethod]
    public void Tangent_ShouldReturnExpectedValue()
    {
        Assert.AreEqual(1d, ScientificCalculator.Tangent(45, AngleUnit.Degrees), 1e-10);
    }

    [TestMethod]
    public void Tangent_ShouldReturnZeroAtZeroRadians()
    {
        Assert.AreEqual(0d, ScientificCalculator.Tangent(0, AngleUnit.Radians), 1e-10);
    }

    [TestMethod]
    public void Tangent_ShouldThrow_WhenUndefined()
    {
        Assert.ThrowsExactly<CalculationException>(() => ScientificCalculator.Tangent(90, AngleUnit.Degrees));
    }

    [TestMethod]
    public void Tangent_ShouldThrow_WhenUndefinedInRadians()
    {
        Assert.ThrowsExactly<CalculationException>(() => ScientificCalculator.Tangent(Math.PI / 2d, AngleUnit.Radians));
    }

    [TestMethod]
    public void ArcSine_ShouldReturnExpectedDegrees()
    {
        Assert.AreEqual(30d, ScientificCalculator.ArcSine(0.5d, AngleUnit.Degrees), 1e-10);
    }

    [TestMethod]
    public void ArcSine_ShouldHandleBoundaryValue()
    {
        Assert.AreEqual(-90d, ScientificCalculator.ArcSine(-1d, AngleUnit.Degrees), 1e-10);
    }

    [TestMethod]
    public void ArcCosine_ShouldReturnExpectedRadians()
    {
        Assert.AreEqual(Math.PI / 3d, ScientificCalculator.ArcCosine(0.5d, AngleUnit.Radians), 1e-10);
    }

    [TestMethod]
    public void ArcCosine_ShouldHandleBoundaryValue()
    {
        Assert.AreEqual(0d, ScientificCalculator.ArcCosine(1d, AngleUnit.Degrees), 1e-10);
    }

    [TestMethod]
    public void ArcTangent_ShouldRespectDegrees()
    {
        Assert.AreEqual(45d, ScientificCalculator.ArcTangent(1d, AngleUnit.Degrees), 1e-10);
    }

    [TestMethod]
    public void ArcTangent_ShouldRespectRadians()
    {
        Assert.AreEqual(Math.PI / 4d, ScientificCalculator.ArcTangent(1d, AngleUnit.Radians), 1e-10);
    }

    [TestMethod]
    public void ArcTangent_ShouldReturnZero()
    {
        Assert.AreEqual(0d, ScientificCalculator.ArcTangent(0d, AngleUnit.Degrees), 1e-10);
    }

    [TestMethod]
    public void InverseTrigonometricFunctions_ShouldThrow_WhenOutsideDomain()
    {
        Assert.ThrowsExactly<CalculationException>(() => ScientificCalculator.ArcSine(1.1d, AngleUnit.Degrees));
        Assert.ThrowsExactly<CalculationException>(() => ScientificCalculator.ArcCosine(-1.1d, AngleUnit.Radians));
    }

    [TestMethod]
    public void NaturalLog_ShouldReturnExpectedValue()
    {
        Assert.AreEqual(0d, ScientificCalculator.NaturalLog(1), 1e-10);
    }

    [TestMethod]
    public void NaturalLog_ShouldHandleEulerNumber()
    {
        Assert.AreEqual(1d, ScientificCalculator.NaturalLog(Math.E), 1e-10);
    }

    [TestMethod]
    public void Base10Log_ShouldReturnExpectedValue()
    {
        Assert.AreEqual(3d, ScientificCalculator.Base10Log(1000), 1e-10);
    }

    [TestMethod]
    public void CustomLog_ShouldReturnExpectedValue()
    {
        Assert.AreEqual(3d, ScientificCalculator.Log(8, 2), 1e-10);
    }

    [TestMethod]
    public void CustomLog_ShouldSupportFractionalBase()
    {
        Assert.AreEqual(2d, ScientificCalculator.Log(0.25d, 0.5d), 1e-10);
    }

    [TestMethod]
    public void Logarithms_ShouldThrow_ForInvalidValuesAndBases()
    {
        Assert.ThrowsExactly<CalculationException>(() => ScientificCalculator.Log(8, 1));
        Assert.ThrowsExactly<CalculationException>(() => ScientificCalculator.NaturalLog(0));
        Assert.ThrowsExactly<CalculationException>(() => ScientificCalculator.Base10Log(-10));
        Assert.ThrowsExactly<CalculationException>(() => ScientificCalculator.Log(16, 0));
        Assert.ThrowsExactly<CalculationException>(() => ScientificCalculator.Log(16, -2));
    }

    [TestMethod]
    public void Logarithms_ShouldRejectBaseNearOne()
    {
        Assert.ThrowsExactly<CalculationException>(() => ScientificCalculator.Log(16, 1.00000000001d));
    }

    [TestMethod]
    public void AddComplex_ShouldReturnExpectedValue()
    {
        var result = ScientificCalculator.AddComplex(new ComplexNumber(3, 2), new ComplexNumber(1, -4));

        Assert.AreEqual(new ComplexNumber(4, -2), result);
    }

    [TestMethod]
    public void MultiplyComplex_ShouldReturnExpectedValue()
    {
        var result = ScientificCalculator.MultiplyComplex(new ComplexNumber(3, 2), new ComplexNumber(1, -4));

        Assert.AreEqual(new ComplexNumber(11, -10), result);
    }

    [TestMethod]
    public void MultiplyComplex_ShouldRespectMultiplicativeIdentity()
    {
        var result = ScientificCalculator.MultiplyComplex(new ComplexNumber(5, -3), new ComplexNumber(1, 0));

        Assert.AreEqual(new ComplexNumber(5, -3), result);
    }

    [TestMethod]
    public void ComplexModulus_ShouldReturnExpectedValue()
    {
        Assert.AreEqual(5d, ScientificCalculator.ComplexModulus(new ComplexNumber(3, 4)), 1e-10);
    }

    [TestMethod]
    public void ComplexModulus_ShouldHandleZero()
    {
        Assert.AreEqual(0d, ScientificCalculator.ComplexModulus(new ComplexNumber(0, 0)), 1e-10);
    }

    [TestMethod]
    public void PolarConversion_ShouldRespectDegrees()
    {
        var polar = ScientificCalculator.ToPolar(new ComplexNumber(1, 1), AngleUnit.Degrees);

        Assert.AreEqual(Math.Sqrt(2), polar.Magnitude, 1e-10);
        Assert.AreEqual(45d, polar.Phase, 1e-10);
        Assert.AreEqual(AngleUnit.Degrees, polar.Unit);
    }

    [TestMethod]
    public void PolarConversion_ShouldRespectRadians()
    {
        var polar = ScientificCalculator.ToPolar(new ComplexNumber(0, 1), AngleUnit.Radians);

        Assert.AreEqual(1d, polar.Magnitude, 1e-10);
        Assert.AreEqual(Math.PI / 2d, polar.Phase, 1e-10);
        Assert.AreEqual(AngleUnit.Radians, polar.Unit);
    }

    [TestMethod]
    public void PolarConversion_ShouldHandleSecondQuadrant()
    {
        var polar = ScientificCalculator.ToPolar(new ComplexNumber(-1, 1), AngleUnit.Degrees);

        Assert.AreEqual(Math.Sqrt(2), polar.Magnitude, 1e-10);
        Assert.AreEqual(135d, polar.Phase, 1e-10);
    }

    [TestMethod]
    public void PolarConversion_ShouldHandleThirdQuadrant()
    {
        var polar = ScientificCalculator.ToPolar(new ComplexNumber(-1, -1), AngleUnit.Degrees);

        Assert.AreEqual(Math.Sqrt(2), polar.Magnitude, 1e-10);
        Assert.AreEqual(-135d, polar.Phase, 1e-10);
    }

    [TestMethod]
    public void Operations_ShouldRejectNaNAndInfinityInputs()
    {
        Assert.ThrowsExactly<CalculationException>(() => ScientificCalculator.Add(double.NaN, 1));
        Assert.ThrowsExactly<CalculationException>(() => ScientificCalculator.Divide(1, double.PositiveInfinity));
        Assert.ThrowsExactly<CalculationException>(() => ScientificCalculator.Power(double.NegativeInfinity, 2));
        Assert.ThrowsExactly<CalculationException>(() => ScientificCalculator.Sine(double.NaN, AngleUnit.Degrees));
        Assert.ThrowsExactly<CalculationException>(() => ScientificCalculator.Log(10, double.NaN));
    }

    [TestMethod]
    public void ComplexOperations_ShouldRejectNonFiniteValues()
    {
        Assert.ThrowsExactly<CalculationException>(() => ScientificCalculator.AddComplex(new ComplexNumber(double.NaN, 1), new ComplexNumber(1, 2)));
        Assert.ThrowsExactly<CalculationException>(() => ScientificCalculator.MultiplyComplex(new ComplexNumber(1, 2), new ComplexNumber(double.PositiveInfinity, 3)));
        Assert.ThrowsExactly<CalculationException>(() => ScientificCalculator.ComplexModulus(new ComplexNumber(double.NegativeInfinity, 1)));
        Assert.ThrowsExactly<CalculationException>(() => ScientificCalculator.ToPolar(new ComplexNumber(1, double.NaN), AngleUnit.Degrees));
    }

    [TestMethod]
    public void OperationRecord_ShouldFormatTimestampLabel()
    {
        var record = new OperationRecord("Basicas", "2 + 2", "4", new DateTime(2026, 3, 14, 9, 8, 7));

        Assert.AreEqual("09:08:07", record.TimestampLabel);
    }

    [TestMethod]
    public void CalculationException_ShouldPreserveMessage()
    {
        var exception = new CalculationException("Mensagem de erro");

        Assert.AreEqual("Mensagem de erro", exception.Message);
    }
}
