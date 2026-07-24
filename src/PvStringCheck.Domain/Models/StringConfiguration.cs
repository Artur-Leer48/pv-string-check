namespace PvStringCheck.Domain.Models;

public sealed record StringConfiguration
{
    public int ModulesPerString { get; init; }
    public int ParallelStringCount { get; init; }
    public double MinimumAmbientTemperatureInDegreesCelsius { get; init; }
}