using Sindika.AspNet.app015.Application.DTOs.Midtrans;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.Midtrans.Contracts;
using Sindika.AspNet.Midtrans.Models.Request.Snap;
using Sindika.AspNet.Midtrans.Models.Common;
using Sindika.AspNet.Midtrans.Models.Notification;
using Microsoft.Extensions.Logging;

namespace Sindika.AspNet.app015.Application.Services
{
    public class MidtransService : IMidtransService
    {
        private readonly IMidtransClient _midtransClient;
        private readonly ILogger<MidtransService> _logger;

        public MidtransService(IMidtransClient midtransClient, ILogger<MidtransService> logger)
        {
            _midtransClient = midtransClient;
            _logger = logger;
        }

        public async Task<PaymentResponseDTO> CreatePaymentAsync(PaymentParam param)
        {
            var snapRequest = new SnapTransactionRequest
            {
                TransactionDetails = new TransactionDetails
                {
                    OrderId = string.IsNullOrEmpty(param.OrderId) ? $"ORDER-{Guid.NewGuid():N}" : param.OrderId,
                    GrossAmount = param.Amount
                },
                CustomerDetails = new CustomerDetails
                {
                    Email = param.Email,
                    FirstName = param.Name
                }
            };

            var response = await _midtransClient.Snap.CreateTransactionAsync(snapRequest);

            return new PaymentResponseDTO
            {
                Token = response.Token,
                RedirectUrl = response.RedirectUrl
            };
        }

        public Task HandleWebhookAsync(MidtransNotification notification)
        {
            _logger.LogInformation("Handling Midtrans webhook for order {OrderId} status {Status}", notification.OrderId, notification.TransactionStatus);

            // TODO: implement domain-specific handling (update orders, publish events, etc.)

            return Task.CompletedTask;
        }
    }
}
