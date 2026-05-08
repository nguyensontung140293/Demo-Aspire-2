namespace Payment.Domain.Entities;

public enum PaymentStatus
{
    Pending = 0,
    Captured = 1,
    Refunded = 2,
    Failed = 3
}
