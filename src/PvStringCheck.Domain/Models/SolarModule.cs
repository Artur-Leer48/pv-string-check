namespace PvStringCheck.Domain.Models;

public sealed record SolarModule
{
    public required string Name { get; init; }
    public int PowerInWatts { get; init; }
    public double OpenCircuitVoltageInVolts { get; init; }
    public double MppVoltageInVolts { get; init; }
    public double ShortCircuitCurrentInAmps { get; init; }
    public double MppCurrentInAmps { get; init; }
    public double OpenCircuitVoltageTemperatureCoefficientPercentPerDegreeCelsius { get; init; }
}