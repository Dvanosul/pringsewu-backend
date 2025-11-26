using Mapster;
using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.app015.API.Models.Midtrans;
using Sindika.AspNet.app015.Application.DTOs.Midtrans;
using Sindika.AspNet.app015.Common.Helpers;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;
using Sindika.AspNet.Midtrans.Contracts;
using Sindika.AspNet.Midtrans.Models.Request.Snap;
using Sindika.AspNet.Midtrans.Models.Common;
using Sindika.AspNet.Midtrans.Models.Notification;
using Sindika.AspNet.Midtrans.Enums;
using Sindika.AspNet.Midtrans.Filters;
using Sindika.AspNet.Midtrans.ModelBinders;
using Sindika.AspNet.Authentication.Attributes;
using Sindika.AspNet.app015.Application.Interfaces.Services;

namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("midtrans", "Midtrans payment...")]
    [PublicScope]
    [ApiController]
    [Route("api/v1/midtrans")]
    public class MidtransController : ControllerBase
    {
        private readonly IMidtransService _midtransService;
        private readonly ILogger<MidtransController> _logger;

        public MidtransController(IMidtransService midtransService, ILogger<MidtransController> logger)
        {
            _midtransService = midtransService;
            _logger = logger;
        }

        [Event("create")]
        [HttpPost("payment")]
        public async Task<IActionResult> CreatePayment([FromBody] BaseRequest<CreatePaymentRequest> request)
        {
            var req = request.Data;

            var snapRequest = new SnapTransactionRequest
            {
                TransactionDetails = new TransactionDetails
                {
                    OrderId = string.IsNullOrEmpty(req.OrderId) ? $"ORDER-{Guid.NewGuid():N}" : req.OrderId,
                    GrossAmount = req.Amount
                },
                CustomerDetails = new CustomerDetails
                {
                    Email = req.Email,
                    FirstName = req.Name
                }
            };

            var param = request.Data.Adapt<PaymentParam>();
            var dto = await _midtransService.CreatePaymentAsync(param);

            return Ok(ResponseHelper.Success<object>(dto, "Create payment token successfully"));
        }

        [Event("webhook")]
        [HttpPost("webhooks/notification")]
        [ValidateMidtransSignature]
        public async Task<IActionResult> HandleWebhook([ModelBinder(BinderType = typeof(MidtransNotificationBinder))] MidtransNotification notification)
        {
            _logger.LogInformation("Received midtrans notification for order {OrderId} status {Status}", notification.OrderId, notification.TransactionStatus);
            
            var result = await _midtransService.HandleWebhookAsync(notification);

            if (result is not null && result.Success)
            {
                return Ok(ResponseHelper.Success<object>(result.Data, "Payment confirmed, donation created successfully"));
            }

            return Ok(ResponseHelper.Success<object>(null, "Webhook processed"));
        }
    }
}
