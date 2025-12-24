using Pharmacy.Domain.Entities.RxClaims;
using Pharmacy.Domain.Entities.PriorAuthorization;

namespace Pharmacy.Domain.Entities.NDCMaintenance
{
    public class NdcProduct
    {
        public int Id { get; set; }
        public string NDC { get; set; } // National Drug Code
        public string ProductName { get; set; }
        public string Manufacturer { get; set; }
        public string DosageForm { get; set; }
        public string Strength { get; set; }
        public string PackageSize { get; set; }
        public bool IsActive { get; set; }

        public ICollection<RxClaim> RxClaims { get; set; }
        public ICollection<PriorAuthorizationRecord> PriorAuthorizations { get; set; }
    }
}
