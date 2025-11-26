using Sindika.AspNet.app015.Application.DTOs.Midtrans;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    /// <summary>
    /// Service for managing pending donations while awaiting payment confirmation.
    /// </summary>
    public interface IPendingDonationService
    {
        /// <summary>
        /// Store a pending donation with the order ID as key.
        /// </summary>
        Task StorePendingDonationAsync(string orderId, PendingDonationDTO donation);

        /// <summary>
        /// Retrieve a pending donation by order ID.
        /// </summary>
        Task<PendingDonationDTO?> GetPendingDonationAsync(string orderId);

        /// <summary>
        /// Remove a pending donation after it's been processed.
        /// </summary>
        Task RemovePendingDonationAsync(string orderId);
    }
}
