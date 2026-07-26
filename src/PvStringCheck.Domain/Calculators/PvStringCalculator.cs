namespace PvStringCheck.Domain.Calculators;

using PvStringCheck.Domain.Models;

public sealed class PvStringCalculator
{
    public CalculationResult Calculate(SolarModule module, Inverter inverter, StringConfiguration configuration)
    {
        var messages = PvStringValidator.ValidateCalculationInputs(inverter, configuration);

        if (messages.Count > 0)
        {
            return new CalculationResult
            {
                Messages = messages,
            };
        }

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

        var totalInputCurrentInAmps = module.MppCurrentInAmps * configuration.ParallelStringCount;

        var totalModulePowerInWatts = module.PowerInWatts * configuration.ModulesPerString * configuration.ParallelStringCount;

        var moduleToInverterPowerRatio = (double)totalModulePowerInWatts / inverter.RatedAcPowerInWatts;

        PvStringValidator.AddElectricalValidationMessages(
            messages,
            inverter,
            stringMppVoltageAtStandardTestConditionsInVolts,
            expectedOpenCircuitVoltageAtMinimumTemperatureInVolts,
            totalInputCurrentInAmps,
            totalModulePowerInWatts,
            moduleToInverterPowerRatio);

        return new CalculationResult
        {
            TotalArrayPowerInKilowattsPeak = totalArrayPowerInKilowattsPeak,
            StringMppVoltageAtStandardTestConditionsInVolts = stringMppVoltageAtStandardTestConditionsInVolts,
            ExpectedOpenCircuitVoltageAtMinimumTemperatureInVolts = expectedOpenCircuitVoltageAtMinimumTemperatureInVolts,
            TotalInputCurrentInAmps = totalInputCurrentInAmps,
            ModuleToInverterPowerRatio = moduleToInverterPowerRatio,
            Messages = messages,
        };
    }
}
