using Pharmacy.Domain.Entities.RxClaims;

namespace Pharmacy.Domain.Entities.Finance
{
    public class FinanceTransaction
    {
        public int Id { get; set; }
        public string TransactionNumber { get; set; }
        public int RxClaimId { get; set; }
        public RxClaim RxClaim { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string Notes { get; set; }
    }
}
