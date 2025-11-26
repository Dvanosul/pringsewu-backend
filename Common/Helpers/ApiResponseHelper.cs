namespace Sindika.AspNet.app015.Common.Helpers
{
    public static class ApiResponseHelper
    {
        public static object Error(string message, string code = "ERR-BLOCKCHAIN-001", object? data = null)
        {
            return new
            {
                success = false,
                code,
                message,
                data
            };
        }
    }
}
