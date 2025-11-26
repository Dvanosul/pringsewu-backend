namespace Sindika.AspNet.app015.Application.DTOs.Midtrans
{
    public class PaymentParam
    {
        public decimal Amount { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? OrderId { get; set; }
    }
}
