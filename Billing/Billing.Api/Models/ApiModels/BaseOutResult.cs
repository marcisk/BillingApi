namespace Billing.Api.Models.ApiModels
{
    public class BaseOutResult
    {
        public bool HasErrors { get; set; }
        public string ErrorMessage { get; set; }
    }
}
