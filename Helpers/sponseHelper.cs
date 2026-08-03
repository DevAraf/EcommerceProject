namespace VendorEcommerceProject.Helpers
{
    public static class ResponseHelper
    {
        public static object SendResponse(this string message, string Status = "Success")
        {
            return new { Status, Message = message };
        }
    }
}
