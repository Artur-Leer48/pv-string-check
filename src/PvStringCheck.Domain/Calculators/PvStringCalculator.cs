namespace PvStringCheck.Domain.Calculators;

using PvStringCheck.Domain.Models;

public sealed class PvStringCalculator
{
    public CalculationResult Calculate(SolarModule module, Inverter inverter, StringConfiguration configuration)
    {
        var totalModules = configuration.ModulesPerString * configuration.ParallelStringCount;

        var totalArrayPowerInKilowattsPeak = module.PowerInWatts * totalModules / 1000.0;

        var stringMppVoltageAtStandardTestConditionsInVolts = module.MppVoltageInVolts * configuration.ModulesPerString;

        return new CalculationResult
        {
            TotalArrayPowerInKilowattsPeak = totalArrayPowerInKilowattsPeak,
            StringMppVoltageAtStandardTestConditionsInVolts = stringMppVoltageAtStandardTestConditionsInVolts
        };
    }
}