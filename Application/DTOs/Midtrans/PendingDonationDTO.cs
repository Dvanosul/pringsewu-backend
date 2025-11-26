namespace Sindika.AspNet.app015.Application.DTOs.Midtrans
{
    /// <summary>
    /// Represents a donation that is pending payment confirmation from Midtrans.
    /// Stored temporarily until webhook confirms payment success.
    /// </summary>
    public class PendingDonationDTO
    {
        public string DonationId { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string Amount { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string EventCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
