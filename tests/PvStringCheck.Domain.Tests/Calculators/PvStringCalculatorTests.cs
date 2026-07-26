using PvStringCheck.Domain.Calculators;

namespace PvStringCheck.Domain.Tests.Calculators;

public sealed class PvStringCalculatorTests
{
    [Fact]
    public void Calculate_WhenArrayPowerIsCalculated_ReturnsCorrectKilowattsPeak()
    {
        // Arrange
        var module = TestData.ValidModule() with { PowerInWatts = 500 };
        var inverter = TestData.ValidInverter();
        var configuration = TestData.ValidConfiguration();
        var calculator = new PvStringCalculator();

        // Act
        var result = calculator.Calculate(module, inverter, configuration);

        // Assert
        Assert.Equal(10, result.TotalArrayPowerInKilowattsPeak);
    }

    [Fact]
    public void Calculate_WhenStringMppVoltageIsCalculated_ReturnsCorrectVoltage()
    {
        // Arrange
        var module = TestData.ValidModule() with { MppVoltageInVolts = 60 };
        var inverter = TestData.ValidInverter();
        var configuration = TestData.ValidConfiguration();
        var calculator = new PvStringCalculator();

        // Act
        var result = calculator.Calculate(module, inverter, configuration);

        // Assert
        Assert.Equal(600, result.StringMppVoltageAtStandardTestConditionsInVolts);
    }

    [Fact]
    public void Calculate_WhenExpectedOpenCircuitVoltageAtMinimumTemperatureIsCalculated_ReturnsCorrectVoltage()
    {
        // Arrange
        var module = TestData.ValidModule() with { OpenCircuitVoltageInVolts = 70 };
        var inverter = TestData.ValidInverter();
        var configuration = TestData.ValidConfiguration();
        var calculator = new PvStringCalculator();

        // Act
        var result = calculator.Calculate(module, inverter, configuration);

        // Assert
        Assert.Equal(773.5, result.ExpectedOpenCircuitVoltageAtMinimumTemperatureInVolts, precision: 2);
    }

    [Fact]
    public void Calculate_WhenTotalInputCurrentIsCalculated_ReturnsCorrectCurrent()
    {
        // Arrange
        var module = TestData.ValidModule() with { MppCurrentInAmps = 8 };
        var inverter = TestData.ValidInverter();
        var configuration = TestData.ValidConfiguration();
        var calculator = new PvStringCalculator();

        // Act
        var result = calculator.Calculate(module, inverter, configuration);

        // Assert
        Assert.Equal(16, result.TotalInputCurrentInAmps);
    }

    [Fact]
    public void Calculate_WhenModuleToInverterPowerRatioIsCalculated_ReturnsCorrectRatio()
    {
        // Arrange
        var module = TestData.ValidModule();
        var inverter = TestData.ValidInverter() with { RatedAcPowerInWatts = 6000 };
        var configuration = TestData.ValidConfiguration();
        var calculator = new PvStringCalculator();

        // Act
        var result = calculator.Calculate(module, inverter, configuration);

        // Assert
        Assert.Equal(1.33, result.ModuleToInverterPowerRatio, precision: 2);
    }
}
