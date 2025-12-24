using Pharmacy.Domain.Entities.PriorAuthorization;
using Pharmacy.Domain.Entities.RxClaims;

namespace Pharmacy.Domain.Entities.Providers
{
    public class Provider
    {
        public int Id { get; set; }
        public string ProviderNumber { get; set; }
        public string Name { get; set; }
        public string NPI { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Specialty { get; set; }

        public ICollection<RxClaim> RxClaims { get; set; }
        public ICollection<PriorAuthorizationRecord> PriorAuthorizations { get; set; }
    }
}
