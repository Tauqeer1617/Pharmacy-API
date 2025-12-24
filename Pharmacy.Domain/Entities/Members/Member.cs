using Pharmacy.Domain.Entities.PriorAuthorization;
using Pharmacy.Domain.Entities.RxClaims;

namespace Pharmacy.Domain.Entities.Members
{
    public class Member
    {
        public int Id { get; set; }
        public string MemberNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public ICollection<RxClaim> RxClaims { get; set; }
        public ICollection<PriorAuthorizationRecord> PriorAuthorizations { get; set; }
    }
}
