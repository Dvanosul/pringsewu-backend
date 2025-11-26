using Sindika.AspNet.app015.Application.DTOs.Midtrans;
using Sindika.AspNet.app015.Application.DTOs.Blockchain.Donation;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Application.Interfaces.Services.Blockchain;
using Sindika.AspNet.Midtrans.Contracts;
using Sindika.AspNet.Midtrans.Models.Request.Snap;
using Sindika.AspNet.Midtrans.Models.Common;
using Sindika.AspNet.Midtrans.Models.Notification;
using Sindika.AspNet.Midtrans.Enums;
using Microsoft.Extensions.Logging;

namespace Sindika.AspNet.app015.Application.Services
{
    public class MidtransService : IMidtransService
    {
        private readonly IMidtransClient _midtransClient;
        private readonly IPendingDonationService _pendingDonationService;
        private readonly IBlockchainDonationService _blockchainDonationService;
        private readonly ILogger<MidtransService> _logger;

        public MidtransService(
            IMidtransClient midtransClient,
            IPendingDonationService pendingDonationService,
            IBlockchainDonationService blockchainDonationService,
            ILogger<MidtransService> logger)
        {
            _midtransClient = midtransClient;
            _pendingDonationService = pendingDonationService;
            _blockchainDonationService = blockchainDonationService;
            _logger = logger;
        }

        public async Task<PaymentResponseDTO> CreatePaymentAsync(PaymentParam param)
        {
            var orderId = string.IsNullOrEmpty(param.OrderId) ? $"ORDER-{Guid.NewGuid():N}" : param.OrderId;
            
            var snapRequest = new SnapTransactionRequest
            {
                TransactionDetails = new TransactionDetails
                {
                    OrderId = orderId,
                    GrossAmount = param.Amount
                },
                CustomerDetails = new CustomerDetails
                {
                    FirstName = param.Name
                }
            };

            var response = await _midtransClient.Snap.CreateTransactionAsync(snapRequest);

            if (param.PendingDonation is not null)
            {
                await _pendingDonationService.StorePendingDonationAsync(orderId, param.PendingDonation);
                _logger.LogInformation("Stored pending donation for order {OrderId}, waiting for payment confirmation", orderId);
            }

            return new PaymentResponseDTO
            {
                Token = response.Token,
                RedirectUrl = response.RedirectUrl
            };
        }

        public async Task<BlockchainDonationSingleResponse?> HandleWebhookAsync(MidtransNotification notification)
        {
            _logger.LogInformation("Handling Midtrans webhook for order {OrderId} status {Status}", 
                notification.OrderId, notification.TransactionStatus);

            if (notification.TransactionStatus == TransactionStatus.Settlement ||
                notification.TransactionStatus == TransactionStatus.Capture)
            {
                var pendingDonation = await _pendingDonationService.GetPendingDonationAsync(notification.OrderId);
                
                if (pendingDonation is not null)
                {
                    _logger.LogInformation("Payment confirmed for order {OrderId}, creating donation in blockchain", notification.OrderId);
                    
                    var result = await _blockchainDonationService.CreateDonationInBlockchainAsync(pendingDonation);
                    
                    if (result.Success)
                    {
                        await _pendingDonationService.RemovePendingDonationAsync(notification.OrderId);
                        _logger.LogInformation("Successfully created donation {DonationId} in blockchain after payment confirmation", 
                            pendingDonation.DonationId);
                    }
                    else
                    {
                        _logger.LogError("Failed to create donation {DonationId} in blockchain: {Message}", 
                            pendingDonation.DonationId, result.Message);
                    }
                    
                    return result;
                }
                else
                {
                    _logger.LogWarning("No pending donation found for order {OrderId}", notification.OrderId);
                }
            }
            else if (notification.TransactionStatus == TransactionStatus.Deny ||
                     notification.TransactionStatus == TransactionStatus.Cancel ||
                     notification.TransactionStatus == TransactionStatus.Expire)
            {
                await _pendingDonationService.RemovePendingDonationAsync(notification.OrderId);
                _logger.LogInformation("Removed pending donation for order {OrderId} due to status {Status}", 
                    notification.OrderId, notification.TransactionStatus);
            }

            return null;
        }
    }
}
