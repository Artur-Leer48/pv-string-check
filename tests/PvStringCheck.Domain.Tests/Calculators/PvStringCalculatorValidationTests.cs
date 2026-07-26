using PvStringCheck.Domain.Calculators;
using PvStringCheck.Domain.Models;

namespace PvStringCheck.Domain.Tests.Calculators;

public sealed class PvStringCalculatorValidationTests
{
    [Fact]
    public void Calculate_WhenConfigurationIsValid_ReturnsNoMessages()
    {
        // Arrange
        var module = TestData.ValidModule();
        var inverter = TestData.ValidInverter();
        var configuration = TestData.ValidConfiguration();
        var calculator = new PvStringCalculator();

        // Act
        var result = calculator.Calculate(module, inverter, configuration);

        // Assert
        Assert.Empty(result.Messages);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Calculate_WhenModulesPerStringIsNotPositive_ReturnsError(int modulesPerString)
    {
        // Arrange
        var module = TestData.ValidModule();
        var inverter = TestData.ValidInverter();
        var configuration = TestData.ValidConfiguration() with { ModulesPerString = modulesPerString };
        var calculator = new PvStringCalculator();

        // Act
        var result = calculator.Calculate(module, inverter, configuration);

        // Assert
        var message = Assert.Single(result.Messages);
        Assert.Equal(ValidationSeverity.Error, message.Severity);
        Assert.Equal("Modules per string must be greater than zero.", message.Message);
    }

    [Fact]
    public void Calculate_WhenMaximumDcVoltageIsExceeded_ReturnsError()
    {
        // Arrange
        var module = TestData.ValidModule();
        var inverter = TestData.ValidInverter() with { MaximumDcVoltageInVolts = 500 };
        var configuration = TestData.ValidConfiguration();
        var calculator = new PvStringCalculator();

        // Act
        var result = calculator.Calculate(module, inverter, configuration);

        // Assert
        var message = Assert.Single(result.Messages);
        Assert.Equal(ValidationSeverity.Error, message.Severity);
        Assert.Equal("Maximum DC voltage is exceeded.", message.Message);
    }
}
