namespace PvStringCheck.Application.Calculations;

public sealed record SolarModuleInput(
    string Name,
    int PowerInWatts,
    double OpenCircuitVoltageInVolts,
    double MppVoltageInVolts,
    double ShortCircuitCurrentInAmps,
    double MppCurrentInAmps,
    double OpenCircuitVoltageTemperatureCoefficientPercentPerDegreeCelsius);
