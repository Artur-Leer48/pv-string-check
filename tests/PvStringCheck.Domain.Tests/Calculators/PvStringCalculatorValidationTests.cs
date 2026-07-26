using PvStringCheck.Domain.Calculators;
using PvStringCheck.Domain.Models;

namespace PvStringCheck.Domain.Tests.Calculators;

public sealed class PvStringCalculatorValidationTests
{
    [Fact]
    public void Calculate_WhenConfigurationIsValid_ReturnsInformationWithoutErrors()
    {
        var result = Calculate();

        Assert.DoesNotContain(result.Messages, message => message.Severity == ValidationSeverity.Error);
        AssertContainsMessage(result, ValidationSeverity.Information, "String voltage is within the MPPT range.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Calculate_WhenModulesPerStringIsNotPositive_ReturnsError(int modulesPerString)
    {
        var configuration = TestData.ValidConfiguration() with { ModulesPerString = modulesPerString };

        var result = Calculate(configuration: configuration);

        AssertContainsMessage(result, ValidationSeverity.Error, "Modules per string must be greater than zero.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Calculate_WhenParallelStringCountIsNotPositive_ReturnsError(int parallelStringCount)
    {
        var configuration = TestData.ValidConfiguration() with { ParallelStringCount = parallelStringCount };

        var result = Calculate(configuration: configuration);

        AssertContainsMessage(result, ValidationSeverity.Error, "Parallel string count must be greater than zero.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Calculate_WhenRatedAcPowerIsNotPositive_ReturnsError(int ratedAcPowerInWatts)
    {
        var inverter = TestData.ValidInverter() with { RatedAcPowerInWatts = ratedAcPowerInWatts };

        var result = Calculate(inverter: inverter);

        AssertContainsMessage(result, ValidationSeverity.Error, "Inverter rated AC power must be greater than zero.");
    }

    [Fact]
    public void Calculate_WhenMpptRangeIsInvalid_ReturnsError()
    {
        var inverter = TestData.ValidInverter() with
        {
            MinimumMpptVoltageInVolts = 801,
            MaximumMpptVoltageInVolts = 800,
        };

        var result = Calculate(inverter: inverter);

        AssertContainsMessage(result, ValidationSeverity.Error, "Inverter minimum MPPT voltage must not exceed its maximum MPPT voltage.");
    }

    [Fact]
    public void Calculate_WhenMaximumDcVoltageIsExceeded_ReturnsError()
    {
        var inverter = TestData.ValidInverter() with { MaximumDcVoltageInVolts = 500 };

        var result = Calculate(inverter: inverter);

        AssertContainsMessage(result, ValidationSeverity.Error, "Maximum DC voltage is exceeded.");
    }

    [Fact]
    public void Calculate_WhenStringMppVoltageIsBelowMpptRange_ReturnsError()
    {
        var inverter = TestData.ValidInverter() with { MinimumMpptVoltageInVolts = 500 };

        var result = Calculate(inverter: inverter);

        AssertContainsMessage(result, ValidationSeverity.Error, "String MPP voltage is below the inverter MPPT range.");
    }

    [Fact]
    public void Calculate_WhenStringMppVoltageIsAboveMpptRange_ReturnsError()
    {
        var inverter = TestData.ValidInverter() with { MaximumMpptVoltageInVolts = 300 };

        var result = Calculate(inverter: inverter);

        AssertContainsMessage(result, ValidationSeverity.Error, "String MPP voltage is above the inverter MPPT range.");
    }

    [Fact]
    public void Calculate_WhenMaximumInputCurrentIsExceeded_ReturnsError()
    {
        var inverter = TestData.ValidInverter() with { MaximumInputCurrentInAmps = 19 };

        var result = Calculate(inverter: inverter);

        AssertContainsMessage(result, ValidationSeverity.Error, "Maximum input current is exceeded.");
    }

    [Fact]
    public void Calculate_WhenMaximumDcPowerIsExceeded_ReturnsError()
    {
        var inverter = TestData.ValidInverter() with { MaximumDcPowerInWatts = 7999 };

        var result = Calculate(inverter: inverter);

        AssertContainsMessage(result, ValidationSeverity.Error, "Maximum DC power is exceeded.");
    }

    [Fact]
    public void Calculate_WhenInverterIsStronglyUndersized_ReturnsWarning()
    {
        var inverter = TestData.ValidInverter() with { RatedAcPowerInWatts = 5000 };

        var result = Calculate(inverter: inverter);

        AssertContainsMessage(result, ValidationSeverity.Warning, "The inverter is strongly undersized.");
    }

    [Fact]
    public void Calculate_WhenPowerRatioEqualsWarningThreshold_ReturnsNoWarning()
    {
        var module = TestData.ValidModule() with { PowerInWatts = 300 };
        var inverter = TestData.ValidInverter() with { RatedAcPowerInWatts = 4000 };

        var result = Calculate(module: module, inverter: inverter);

        Assert.DoesNotContain(result.Messages, message =>
            message.Severity == ValidationSeverity.Warning
            && message.Message == "The inverter is strongly undersized.");
    }

    [Fact]
    public void Calculate_WhenValuesEqualInverterLimits_ReturnsNoLimitErrorsOrWarnings()
    {
        var inverter = TestData.ValidInverter() with
        {
            MaximumDcVoltageInVolts = 500,
            MinimumMpptVoltageInVolts = 400,
            MaximumMpptVoltageInVolts = 400,
            MaximumInputCurrentInAmps = 20,
            MaximumDcPowerInWatts = 8000,
        };
        var configuration = TestData.ValidConfiguration() with
        {
            MinimumAmbientTemperatureInDegreesCelsius = 25,
        };

        var result = Calculate(inverter: inverter, configuration: configuration);

        Assert.DoesNotContain(result.Messages, message => message.Severity is ValidationSeverity.Error or ValidationSeverity.Warning);
        AssertContainsMessage(result, ValidationSeverity.Information, "String voltage is within the MPPT range.");
    }

    private static CalculationResult Calculate(
        SolarModule? module = null,
        Inverter? inverter = null,
        StringConfiguration? configuration = null)
    {
        var calculator = new PvStringCalculator();

        return calculator.Calculate(
            module ?? TestData.ValidModule(),
            inverter ?? TestData.ValidInverter(),
            configuration ?? TestData.ValidConfiguration());
    }

    private static void AssertContainsMessage(
        CalculationResult result,
        ValidationSeverity severity,
        string text)
    {
        Assert.Contains(result.Messages, message =>
            message.Severity == severity && message.Message == text);
    }
}
