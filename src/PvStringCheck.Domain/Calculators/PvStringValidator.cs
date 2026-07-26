namespace PvStringCheck.Domain.Calculators;

using PvStringCheck.Domain.Models;

internal static class PvStringValidator
{
    private const double StronglyUndersizedPowerRatioThreshold = 1.5;

    public static List<ValidationMessage> ValidateCalculationInputs(
        Inverter inverter,
        StringConfiguration configuration)
    {
        List<ValidationMessage> messages = [];

        if (configuration.ModulesPerString <= 0)
        {
            messages.Add(new ValidationMessage(
                ValidationSeverity.Error,
                "Modules per string must be greater than zero."));
        }

        if (configuration.ParallelStringCount <= 0)
        {
            messages.Add(new ValidationMessage(
                ValidationSeverity.Error,
                "Parallel string count must be greater than zero."));
        }

        if (inverter.RatedAcPowerInWatts <= 0)
        {
            messages.Add(new ValidationMessage(
                ValidationSeverity.Error,
                "Inverter rated AC power must be greater than zero."));
        }

        return messages;
    }

    public static void AddElectricalValidationMessages(
        List<ValidationMessage> messages,
        Inverter inverter,
        double stringMppVoltageInVolts,
        double expectedOpenCircuitVoltageAtMinimumTemperatureInVolts,
        double totalInputCurrentInAmps,
        int totalModulePowerInWatts,
        double moduleToInverterPowerRatio)
    {
        if (expectedOpenCircuitVoltageAtMinimumTemperatureInVolts > inverter.MaximumDcVoltageInVolts)
        {
            messages.Add(new ValidationMessage(
                ValidationSeverity.Error,
                "Maximum DC voltage is exceeded."));
        }

        if (inverter.MinimumMpptVoltageInVolts > inverter.MaximumMpptVoltageInVolts)
        {
            messages.Add(new ValidationMessage(
                ValidationSeverity.Error,
                "Inverter minimum MPPT voltage must not exceed its maximum MPPT voltage."));
        }
        else if (stringMppVoltageInVolts < inverter.MinimumMpptVoltageInVolts)
        {
            messages.Add(new ValidationMessage(
                ValidationSeverity.Error,
                "String MPP voltage is below the inverter MPPT range."));
        }
        else if (stringMppVoltageInVolts > inverter.MaximumMpptVoltageInVolts)
        {
            messages.Add(new ValidationMessage(
                ValidationSeverity.Error,
                "String MPP voltage is above the inverter MPPT range."));
        }
        else
        {
            messages.Add(new ValidationMessage(
                ValidationSeverity.Information,
                "String voltage is within the MPPT range."));
        }

        if (totalInputCurrentInAmps > inverter.MaximumInputCurrentInAmps)
        {
            messages.Add(new ValidationMessage(
                ValidationSeverity.Error,
                "Maximum input current is exceeded."));
        }

        if (totalModulePowerInWatts > inverter.MaximumDcPowerInWatts)
        {
            messages.Add(new ValidationMessage(
                ValidationSeverity.Error,
                "Maximum DC power is exceeded."));
        }

        if (moduleToInverterPowerRatio > StronglyUndersizedPowerRatioThreshold)
        {
            messages.Add(new ValidationMessage(
                ValidationSeverity.Warning,
                "The inverter is strongly undersized."));
        }
    }
}
