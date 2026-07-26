namespace PvStringCheck.Domain.Calculators;

using PvStringCheck.Domain.Models;

public sealed class PvStringCalculator
{
    public CalculationResult Calculate(SolarModule module, Inverter inverter, StringConfiguration configuration)
    {
        if (configuration.ModulesPerString <= 0)
        {
            return new CalculationResult
            {
                Messages =
                [
                    new ValidationMessage(
                        ValidationSeverity.Error,
                        "Modules per string must be greater than zero.")
                ],
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

        List<ValidationMessage> messages = [];

        if (expectedOpenCircuitVoltageAtMinimumTemperatureInVolts > inverter.MaximumDcVoltageInVolts)
        {
            messages.Add(new ValidationMessage(
                ValidationSeverity.Error,
                "Maximum DC voltage is exceeded."));
        }

        var totalInputCurrentInAmps = module.MppCurrentInAmps * configuration.ParallelStringCount;

        var totalModulePower = module.PowerInWatts * configuration.ModulesPerString * configuration.ParallelStringCount;

        var moduleToInverterPowerRatio = (double)totalModulePower / inverter.RatedAcPowerInWatts;

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
