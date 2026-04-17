namespace StoreOperationsService.Domain;

public sealed record OperationalModifiers(
    bool IsRush,
    bool IsAtRisk,
    PriorityBand PriorityBand)
{
    public static OperationalModifiers Default =>
        new(IsRush: false, IsAtRisk: false, PriorityBand: PriorityBand.Standard);
}
