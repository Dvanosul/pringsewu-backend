using System;

namespace Sindika.AspNet.app015.API.Models.Midtrans
{
    public class CreatePaymentRequest
    {
        public decimal Amount { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? OrderId { get; set; }
    }
}
