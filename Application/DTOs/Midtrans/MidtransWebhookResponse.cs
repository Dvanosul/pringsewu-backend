namespace Sindika.AspNet.app015.Application.DTOs.Midtrans
{
    public class MidtransWebhookResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
    }
}
