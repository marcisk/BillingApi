namespace Billing.Api.Models.ApiModels
{
    public class ProcessOrderOut : BaseOutResult
    {
        public byte[] Receipt { get; set; }
    }
}
