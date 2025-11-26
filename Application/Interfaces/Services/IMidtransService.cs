using Sindika.AspNet.app015.Application.DTOs.Midtrans;
using Sindika.AspNet.Midtrans.Models.Notification;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    public interface IMidtransService
    {
        Task<PaymentResponseDTO> CreatePaymentAsync(PaymentParam param);
        Task HandleWebhookAsync(MidtransNotification notification);
    }
}
