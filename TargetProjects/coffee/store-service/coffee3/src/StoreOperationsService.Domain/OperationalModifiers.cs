namespace StoreOperationsService.Domain;

public sealed record OperationalModifiers(
    bool IsRush,
    bool IsAtRisk,
    PriorityBand PriorityBand)
{
    public static OperationalModifiers Default =>
        new(IsRush: false, IsAtRisk: false, PriorityBand: PriorityBand.Standard);

    public OperationalModifiers WithRush(bool value) =>
        this with { IsRush = value };

    public OperationalModifiers WithAtRisk(bool value) =>
        this with { IsAtRisk = value };
}
