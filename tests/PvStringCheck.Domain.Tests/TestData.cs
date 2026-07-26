using PvStringCheck.Domain.Models;

namespace PvStringCheck.Domain.Tests;

public static class TestData
{
    public static SolarModule ValidModule() => new()
    {
        Name = "Test Module",
        PowerInWatts = 400,
        OpenCircuitVoltageInVolts = 50,
        MppVoltageInVolts = 40,
        ShortCircuitCurrentInAmps = 11,
        MppCurrentInAmps = 10,
        OpenCircuitVoltageTemperatureCoefficientPercentPerDegreeCelsius = -0.3,
    };

    public static Inverter ValidInverter() => new()
    {
        Name = "Test Inverter",
        MaximumDcVoltageInVolts = 1000,
        MinimumMpptVoltageInVolts = 200,
        MaximumMpptVoltageInVolts = 800,
        MaximumInputCurrentInAmps = 30,
        MaximumDcPowerInWatts = 10000,
        RatedAcPowerInWatts = 8000,
    };

    public static StringConfiguration ValidConfiguration() => new()
    {
        ModulesPerString = 10,
        ParallelStringCount = 2,
        MinimumAmbientTemperatureInDegreesCelsius = -10,
    };
}
