namespace PvStringCheck.Domain.Models;

public sealed record ValidationMessage(ValidationSeverity Severity, string Message);
