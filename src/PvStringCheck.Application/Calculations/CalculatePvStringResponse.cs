namespace PvStringCheck.Application.Calculations;

public sealed record CalculatePvStringResponse(
    double TotalArrayPowerInKilowattsPeak,
    double StringMppVoltageAtStandardTestConditionsInVolts,
    double ExpectedOpenCircuitVoltageAtMinimumTemperatureInVolts,
    double TotalInputCurrentInAmps,
    double ModuleToInverterPowerRatio,
    IReadOnlyList<ValidationMessageOutput> Messages);
