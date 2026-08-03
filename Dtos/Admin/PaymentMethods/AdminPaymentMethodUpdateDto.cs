namespace EcommerceProject.Dtos.Admin.PaymentMethods
{
    public class AdminPaymentMethodUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
