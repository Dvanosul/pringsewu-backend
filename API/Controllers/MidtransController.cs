using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.app015.API.Models.Midtrans;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Common.Helpers;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;
using Sindika.AspNet.Midtrans.Models.Notification;
using Sindika.AspNet.Midtrans.Filters;
using Sindika.AspNet.Midtrans.ModelBinders;
using Sindika.AspNet.Authentication.Attributes;

namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("midtrans", "Midtrans payment...")]
    [PublicScope]
    [ApiController]
    [Route("api/v1/midtrans")]
    public class MidtransController : ControllerBase
    {
        private readonly IMidtransService _midtransService;

        public MidtransController(IMidtransService midtransService)
        {
            _midtransService = midtransService;
        }

        [Event("insert")]
        [HttpPost("payment")]
        public async Task<IActionResult> CreatePayment([FromBody] BaseRequest<CreatePaymentRequest> request)
        {
            var response = await _midtransService.CreatePaymentAsync(request.Data);
            return Ok(ResponseHelper.Success<object>(response, "Create payment token successfully"));
        }

        [Event("webhook")]
        [HttpPost("webhooks/notification")]
        [ValidateMidtransSignature]
        public async Task<IActionResult> HandleWebhook([ModelBinder(BinderType = typeof(MidtransNotificationBinder))] MidtransNotification notification)
        {
            var response = await _midtransService.HandleWebhookAsync(notification);
            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }
    }
}
