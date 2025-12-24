using Pharmacy.Application.DTOs.Members;

namespace Pharmacy.Application.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberDto>> GetAllMembersAsync();
        Task<MemberDto?> GetMemberByIdAsync(int id);
        Task<MemberDto?> GetMemberByMemberNumberAsync(string memberNumber);
        Task<IEnumerable<MemberDto>> GetMembersByGenderAsync(string gender);
        Task<IEnumerable<MemberDto>> SearchMembersAsync(string searchTerm);
        Task<MemberSearchResultDto> AdvancedSearchMembersAsync(MemberSearchCriteriaDto searchCriteria);
        Task<MemberDto> CreateMemberAsync(CreateMemberDto createMemberDto);
        Task<MemberDto?> UpdateMemberAsync(int id, UpdateMemberDto updateMemberDto);
        Task<bool> DeleteMemberAsync(int id);
        Task<bool> IsMemberNumberExistsAsync(string memberNumber);
    }
}