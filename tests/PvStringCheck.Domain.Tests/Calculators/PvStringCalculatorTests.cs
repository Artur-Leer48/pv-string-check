using PvStringCheck.Domain.Calculators;
using PvStringCheck.Domain.Models;

namespace PvStringCheck.Domain.Tests.Calculators;

public sealed class PvStringCalculatorTests
{
    [Fact]
    public void Calculate_WhenArrayPowerIsCalculated_ReturnsCorrectKilowattsPeak()
    {
        // Arrange
        var module = new SolarModule { Name = "Test Module", PowerInWatts = 500 };

        var inverter = new Inverter
        {
            Name = "Test Inverter",
            RatedAcPowerInWatts = 6000
        };

        var configuration = new StringConfiguration { ModulesPerString = 5, ParallelStringCount = 2 };

        var calculator = new PvStringCalculator();

        // Act
        var result = calculator.Calculate(module, inverter, configuration);

        // Assert
        Assert.Equal(5, result.TotalArrayPowerInKilowattsPeak);
    }

    [Fact]
    public void Calculate_WhenStringMppVoltageIsCalculated_ReturnsCorrectVoltage()
    {
        // Arrange
        var module = new SolarModule { Name = "Test Module", MppVoltageInVolts = 100 };

        var inverter = new Inverter
        {
            Name = "Test Inverter",
            RatedAcPowerInWatts = 6000
        };

        var configuration = new StringConfiguration { ModulesPerString = 5, ParallelStringCount = 2 };

        var calculator = new PvStringCalculator();

        // Act
        var result = calculator.Calculate(module, inverter, configuration);

        // Assert
        Assert.Equal(500, result.StringMppVoltageAtStandardTestConditionsInVolts);
    }

    [Fact]
    public void Calculate_WhenExpectedOpenCircuitVoltageAtMinimumTemperatureIsCalculated_ReturnsCorrectVoltage()
    {
        // Arrange
        var module = new SolarModule { Name = "Test Module", OpenCircuitVoltageInVolts = 100, OpenCircuitVoltageTemperatureCoefficientPercentPerDegreeCelsius = -0.3 };

        var inverter = new Inverter
        {
            Name = "Test Inverter",
            RatedAcPowerInWatts = 6000
        };

        var configuration = new StringConfiguration { ModulesPerString = 5, ParallelStringCount = 2, MinimumAmbientTemperatureInDegreesCelsius = 15 };

        var calculator = new PvStringCalculator();

        // Act
        var result = calculator.Calculate(module, inverter, configuration);

        // Assert
        Assert.Equal(515, result.ExpectedOpenCircuitVoltageAtMinimumTemperatureInVolts, precision: 2);
    }

    [Fact]
    public void Calculate_WhenTotalInputCurrentIsCalculated_ReturnsCorrectCurrent()
    {
        // Arrange
        var module = new SolarModule { Name = "Test Module", MppCurrentInAmps = 10 };

        var inverter = new Inverter
        {
            Name = "Test Inverter",
            RatedAcPowerInWatts = 6000
        };

        var configuration = new StringConfiguration { ModulesPerString = 5, ParallelStringCount = 2 };

        var calculator = new PvStringCalculator();

        // Act
        var result = calculator.Calculate(module, inverter, configuration);

        // Assert
        Assert.Equal(20, result.TotalInputCurrentInAmps);
    }

    [Fact]
    public void Calculate_WhenModuleToInverterPowerRatioIsCalculated_ReturnsCorrectRatio()
    {
        // Arrange
        SolarModule module = new() { Name = "Test Module", PowerInWatts = 400 };

        Inverter inverter = new() { Name = "Test Inverter", RatedAcPowerInWatts = 6000 };

        StringConfiguration configuration = new() { ModulesPerString = 10, ParallelStringCount = 2 };

        PvStringCalculator calculator = new();

        // Act
        var result = calculator.Calculate(module, inverter, configuration);

        // Assert
        Assert.Equal(1.33, result.ModuleToInverterPowerRatio, precision: 2);
    }
}
