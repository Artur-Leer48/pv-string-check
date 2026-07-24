namespace PvStringCheck.Domain.Models;

public sealed record CalculationResult
{
    public double TotalArrayPowerInKilowattsPeak { get; init; }
    public double StringMppVoltageAtStandardTestConditionsInVolts { get; init; }
    public double ExpectedOpenCircuitVoltageAtMinimumTemperatureInVolts { get; init; }
    public double TotalInputCurrentInAmps { get; init; }
    public double ModuleToInverterPowerRatio { get; init; }
    public IReadOnlyList<ValidationMessage> Messages { get; init; } = [];
}