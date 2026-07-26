namespace PvStringCheck.Application.Calculations;

public sealed record InverterInput(
    string Name,
    int MaximumDcVoltageInVolts,
    int MinimumMpptVoltageInVolts,
    int MaximumMpptVoltageInVolts,
    double MaximumInputCurrentInAmps,
    int MaximumDcPowerInWatts,
    int RatedAcPowerInWatts);
