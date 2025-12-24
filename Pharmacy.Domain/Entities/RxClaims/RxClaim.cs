using Pharmacy.Domain.Entities.ClaimSubmit;
using Pharmacy.Domain.Entities.Finance;
using Pharmacy.Domain.Entities.Members;
using Pharmacy.Domain.Entities.NDCMaintenance;
using Pharmacy.Domain.Entities.Providers;
using Pharmacy.Domain.Entities.RxDUR;

namespace Pharmacy.Domain.Entities.RxClaims
{
    public class RxClaim
    {
        public int Id { get; set; }
        public string ClaimNumber { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; }

        public int ProviderId { get; set; }
        public Provider Provider { get; set; }

        public int NdcProductId { get; set; }
        public NdcProduct NdcProduct { get; set; }

        public DateTime DateOfService { get; set; }
        public int Quantity { get; set; }
        public decimal AmountBilled { get; set; }
        public decimal AmountPaid { get; set; }
        public string Status { get; set; }

        public ICollection<RxDURRecord> RxDURs { get; set; }
        public ICollection<ClaimSubmitRecord> ClaimSubmissions { get; set; }
        public ICollection<FinanceTransaction> FinanceTransactions { get; set; }
    }
}
