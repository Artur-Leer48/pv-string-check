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
            Name = "Test Inverter"
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
            Name = "Test Inverter"
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
            Name = "Test Inverter"
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
            Name = "Test Inverter"
        };

        var configuration = new StringConfiguration { ModulesPerString = 5, ParallelStringCount = 2 };

        var calculator = new PvStringCalculator();

        // Act
        var result = calculator.Calculate(module, inverter, configuration);

        // Assert
        Assert.Equal(20, result.TotalInputCurrentInAmps);
    }
}
