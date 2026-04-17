namespace StoreOperationsService.Domain;

/// <summary>
/// Fixed set of valid reason codes for order cancellation, as defined in decisions.md Q5.
/// Unrecognised values are rejected at the domain boundary.
/// </summary>
public enum CancellationReasonCode
{
    /// <summary>Customer requested cancellation before or during preparation.</summary>
    CustomerRequest,

    /// <summary>Required item is out of stock or cannot be prepared.</summary>
    ItemUnavailable,

    /// <summary>Order exceeded maximum wait threshold without progressing.</summary>
    OrderTimeout,

    /// <summary>Internal error or equipment failure prevents fulfilment.</summary>
    OperationalError,

    /// <summary>Automated cancellation by an upstream or orchestrating system.</summary>
    SystemCancellation,
}
