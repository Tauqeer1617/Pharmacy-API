using Pharmacy.Domain.Entities.Members;

namespace Pharmacy.Domain.Interfaces
{
    public interface IMemberRepository : IGenericRepository<Member>
    {
        Task<Member?> GetByMemberNumberAsync(string memberNumber);
        Task<IEnumerable<Member>> GetMembersByGenderAsync(string gender);
        Task<IEnumerable<Member>> SearchMembersAsync(string searchTerm);
        Task<bool> IsMemberNumberExistsAsync(string memberNumber);
        Task<(IEnumerable<Member> Members, int TotalCount)> AdvancedSearchAsync(
            string? memberNumber = null,
            string? firstName = null,
            string? lastName = null,
            string? email = null,
            string? phone = null,
            string? gender = null,
            string? address = null,
            DateTime? dateOfBirthFrom = null,
            DateTime? dateOfBirthTo = null,
            int? ageFrom = null,
            int? ageTo = null,
            string sortBy = "Id",
            bool sortDescending = false,
            int pageNumber = 1,
            int pageSize = 10);
    }
}