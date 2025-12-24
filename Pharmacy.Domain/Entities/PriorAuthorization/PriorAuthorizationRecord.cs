using Pharmacy.Domain.Entities.Members;
using Pharmacy.Domain.Entities.NDCMaintenance;
using Pharmacy.Domain.Entities.Providers;

namespace Pharmacy.Domain.Entities.PriorAuthorization
{
    public class PriorAuthorizationRecord
    {
        public int Id { get; set; }
        public string AuthorizationNumber { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; }

        public int ProviderId { get; set; }
        public Provider Provider { get; set; }

        public int NdcProductId { get; set; }
        public NdcProduct NdcProduct { get; set; }

        public DateTime RequestedDate { get; set; }
        public string Status { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
