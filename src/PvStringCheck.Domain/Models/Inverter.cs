namespace PvStringCheck.Domain.Models;

public sealed record Inverter
{
    public required string Name { get; init; }
    public int MaximumDcVoltageInVolts { get; init; }
    public int MinimumMpptVoltageInVolts { get; init; }
    public int MaximumMpptVoltageInVolts { get; init; }
    public double MaximumInputCurrentInAmps { get; init; }
    public int MaximumDcPowerInWatts { get; init; }
}