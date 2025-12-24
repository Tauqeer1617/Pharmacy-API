using Pharmacy.Domain.Entities.RxClaims;

namespace Pharmacy.Domain.Entities.RxDUR
{
    public class RxDURRecord
    {
        public int Id { get; set; }
        public int RxClaimId { get; set; }
        public RxClaim RxClaim { get; set; }

        public string AlertCode { get; set; }
        public string AlertDescription { get; set; }
        public DateTime AlertDate { get; set; }
        public string Outcome { get; set; }
    }
}