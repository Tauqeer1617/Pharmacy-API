using Pharmacy.Domain.Entities.RxClaims;

namespace Pharmacy.Domain.Entities.ClaimSubmit
{
    public class ClaimSubmitRecord
    {
        public int Id { get; set; }
        public int RxClaimId { get; set; }
        public RxClaim RxClaim { get; set; }

        public DateTime SubmissionDate { get; set; }
        public string SubmittedBy { get; set; }
        public string SubmissionStatus { get; set; }
    }
}
