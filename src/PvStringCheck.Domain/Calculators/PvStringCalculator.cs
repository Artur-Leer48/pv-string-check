namespace PvStringCheck.Domain.Calculators;

using PvStringCheck.Domain.Models;

public sealed class PvStringCalculator
{
    public CalculationResult Calculate(SolarModule module, Inverter inverter, StringConfiguration configuration)
    {
        var totalModules = configuration.ModulesPerString * configuration.ParallelStringCount;

        var totalArrayPowerInKilowattsPeak = module.PowerInWatts * totalModules / 1000.0;
        var stringMppVoltageAtStandardTestConditionsInVolts = module.MppVoltageInVolts * configuration.ModulesPerString;


        var temperatureDifferenceInDegreesCelsius =
        configuration.MinimumAmbientTemperatureInDegreesCelsius - 25.0;

        var voltageTemperatureFactor =
        1 + module.OpenCircuitVoltageTemperatureCoefficientPercentPerDegreeCelsius
        / 100.0
        * temperatureDifferenceInDegreesCelsius;

        var expectedOpenCircuitVoltageAtMinimumTemperatureInVolts = module.OpenCircuitVoltageInVolts * configuration.ModulesPerString * voltageTemperatureFactor;


        return new CalculationResult
        {
            TotalArrayPowerInKilowattsPeak = totalArrayPowerInKilowattsPeak,
            StringMppVoltageAtStandardTestConditionsInVolts = stringMppVoltageAtStandardTestConditionsInVolts,
            ExpectedOpenCircuitVoltageAtMinimumTemperatureInVolts = expectedOpenCircuitVoltageAtMinimumTemperatureInVolts,
        };
    }
}