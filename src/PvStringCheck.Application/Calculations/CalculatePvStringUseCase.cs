using PvStringCheck.Domain.Calculators;
using PvStringCheck.Domain.Models;

namespace PvStringCheck.Application.Calculations;

public sealed class CalculatePvStringUseCase
{
    private readonly PvStringCalculator _calculator;

    public CalculatePvStringUseCase(PvStringCalculator calculator)
    {
        _calculator = calculator;
    }

    public CalculatePvStringResponse Execute(CalculatePvStringRequest request)
    {
        var (moduleInput, inverterInput, configurationInput) = request;

        SolarModule module = new()
        {
            Name = moduleInput.Name,
            PowerInWatts = moduleInput.PowerInWatts,
            OpenCircuitVoltageInVolts = moduleInput.OpenCircuitVoltageInVolts,
            MppVoltageInVolts = moduleInput.MppVoltageInVolts,
            ShortCircuitCurrentInAmps = moduleInput.ShortCircuitCurrentInAmps,
            MppCurrentInAmps = moduleInput.MppCurrentInAmps,
            OpenCircuitVoltageTemperatureCoefficientPercentPerDegreeCelsius = moduleInput.OpenCircuitVoltageTemperatureCoefficientPercentPerDegreeCelsius,
        };

        Inverter inverter = new()
        {
            Name = inverterInput.Name,
            MaximumDcVoltageInVolts = inverterInput.MaximumDcVoltageInVolts,
            MinimumMpptVoltageInVolts = inverterInput.MinimumMpptVoltageInVolts,
            MaximumMpptVoltageInVolts = inverterInput.MaximumMpptVoltageInVolts,
            MaximumInputCurrentInAmps = inverterInput.MaximumInputCurrentInAmps,
            MaximumDcPowerInWatts = inverterInput.MaximumDcPowerInWatts,
            RatedAcPowerInWatts = inverterInput.RatedAcPowerInWatts,
        };

        StringConfiguration configuration = new()
        {
            ModulesPerString = configurationInput.ModulesPerString,
            ParallelStringCount = configurationInput.ParallelStringCount,
            MinimumAmbientTemperatureInDegreesCelsius = configurationInput.MinimumAmbientTemperatureInDegreesCelsius,
        };

        var result = _calculator.Calculate(module, inverter, configuration);

        var messages = result.Messages
            .Select(message => new ValidationMessageOutput(
                message.Severity.ToString(),
                message.Message))
            .ToArray();

        return new CalculatePvStringResponse(
            result.TotalArrayPowerInKilowattsPeak,
            result.StringMppVoltageAtStandardTestConditionsInVolts,
            result.ExpectedOpenCircuitVoltageAtMinimumTemperatureInVolts,
            result.TotalInputCurrentInAmps,
            result.ModuleToInverterPowerRatio,
            messages);
    }
}
