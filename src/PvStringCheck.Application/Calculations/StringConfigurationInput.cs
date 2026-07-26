namespace PvStringCheck.Application.Calculations;

public sealed record StringConfigurationInput(
    int ModulesPerString,
    int ParallelStringCount,
    double MinimumAmbientTemperatureInDegreesCelsius);
