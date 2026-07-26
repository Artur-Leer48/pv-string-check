namespace PvStringCheck.Application.Calculations;

public sealed record CalculatePvStringRequest(
    SolarModuleInput Module,
    InverterInput Inverter,
    StringConfigurationInput Configuration);
