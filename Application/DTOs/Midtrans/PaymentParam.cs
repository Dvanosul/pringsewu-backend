namespace Sindika.AspNet.app015.Application.DTOs.Midtrans
{
    public class PaymentParam
    {
        public decimal Amount { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? OrderId { get; set; }

        /// <summary>
        /// Pending donation data to be stored while awaiting payment confirmation.
        /// If set, donation will be created in blockchain only after payment is confirmed.
        /// </summary>
        public PendingDonationDTO? PendingDonation { get; set; }
    }
}
