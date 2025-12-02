using Sindika.AspNet.app015.API.Models.Midtrans;
using Sindika.AspNet.app015.Application.DTOs.Midtrans;
using Sindika.AspNet.app015.Application.DTOs.Blockchain.Donation;
using Sindika.AspNet.Midtrans.Models.Notification;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    public interface IMidtransService
    {
        /// <summary>
        /// Creates a payment token. If PaymentParam contains PendingDonation data,
        /// the donation will be stored as pending and created in blockchain when payment is confirmed.
        /// </summary>
        Task<PaymentResponseDTO> CreatePaymentAsync(CreatePaymentRequest request);
        
        /// <summary>
        /// Handles Midtrans webhook notifications.
        /// If payment is successful (settlement), creates the donation in blockchain.
        /// </summary>
        Task<MidtransWebhookResponse> HandleWebhookAsync(MidtransNotification notification);
    }
}
