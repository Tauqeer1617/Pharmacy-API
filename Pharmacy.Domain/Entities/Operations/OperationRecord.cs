using Pharmacy.Domain.Entities.RxClaims;

namespace Pharmacy.Domain.Entities.Operations
{
    public class OperationRecord
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public string OperationType { get; set; }
        public DateTime OperationDate { get; set; }
        public string Details { get; set; }

        // E.g., link to claim/transaction/member/etc if needed (optional navigation)
        public int? RxClaimId { get; set; }
        public RxClaim RxClaim { get; set; }
    }
}
