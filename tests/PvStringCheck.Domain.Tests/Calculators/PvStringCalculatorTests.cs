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
}
