using Pharmacy.Application.DTOs.Members;
using Pharmacy.Application.Services.Interfaces;
using Pharmacy.Domain.Entities.Members;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Services;
using System.Text.Json;

namespace Pharmacy.Application.Services.Implementations
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RedisCacheService _redisCache;

        public MemberService(IUnitOfWork unitOfWork, RedisCacheService redisCache)
        {
            _unitOfWork = unitOfWork;
            _redisCache = redisCache;
        }

        public async Task<IEnumerable<MemberDto>> GetAllMembersAsync()
        {
            var members = await _unitOfWork.Members.GetAllAsync();
            return members.Select(MapToDto);
        }

        public async Task<MemberDto?> GetMemberByIdAsync(int id)
        {
            var cacheKey = $"member:{id}";
            var cached = await _redisCache.GetStringAsync(cacheKey);
            if (cached != null)
                return JsonSerializer.Deserialize<MemberDto>(cached);

            var member = await _unitOfWork.Members.GetByIdAsync(id);
            var dto = member != null ? MapToDto(member) : null;

            if (dto != null)
                await _redisCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(dto));

            return dto;
        }

        public async Task<MemberDto?> GetMemberByMemberNumberAsync(string memberNumber)
        {
            var member = await _unitOfWork.Members.GetByMemberNumberAsync(memberNumber);
            return member != null ? MapToDto(member) : null;
        }

        public async Task<IEnumerable<MemberDto>> GetMembersByGenderAsync(string gender)
        {
            var members = await _unitOfWork.Members.GetMembersByGenderAsync(gender);
            return members.Select(MapToDto);
        }

        public async Task<IEnumerable<MemberDto>> SearchMembersAsync(string searchTerm)
        {
            var members = await _unitOfWork.Members.SearchMembersAsync(searchTerm);
            return members.Select(MapToDto);
        }

        public async Task<MemberSearchResultDto> AdvancedSearchMembersAsync(MemberSearchCriteriaDto searchCriteria)
        {
            var (members, totalCount) = await _unitOfWork.Members.AdvancedSearchAsync(
                memberNumber: searchCriteria.MemberNumber,
                firstName: searchCriteria.FirstName,
                lastName: searchCriteria.LastName,
                email: searchCriteria.Email,
                phone: searchCriteria.Phone,
                gender: searchCriteria.Gender,
                address: searchCriteria.Address,
                dateOfBirthFrom: searchCriteria.DateOfBirthFrom,
                dateOfBirthTo: searchCriteria.DateOfBirthTo,
                ageFrom: searchCriteria.AgeFrom,
                ageTo: searchCriteria.AgeTo,
                sortBy: searchCriteria.SortBy ?? "Id",
                sortDescending: searchCriteria.SortDescending,
                pageNumber: searchCriteria.PageNumber,
                pageSize: searchCriteria.PageSize);

            var totalPages = (int)Math.Ceiling(totalCount / (double)searchCriteria.PageSize);

            return new MemberSearchResultDto
            {
                Members = members.Select(MapToDto),
                TotalCount = totalCount,
                PageNumber = searchCriteria.PageNumber,
                PageSize = searchCriteria.PageSize,
                TotalPages = totalPages,
                HasPreviousPage = searchCriteria.PageNumber > 1,
                HasNextPage = searchCriteria.PageNumber < totalPages
            };
        }

        private IEnumerable<Member> ApplyFilters(IEnumerable<Member> members, MemberSearchCriteriaDto criteria)
        {
            if (!string.IsNullOrEmpty(criteria.MemberNumber))
            {
                members = members.Where(m => m.MemberNumber.Contains(criteria.MemberNumber, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(criteria.FirstName))
            {
                members = members.Where(m => m.FirstName.Contains(criteria.FirstName, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(criteria.LastName))
            {
                members = members.Where(m => m.LastName.Contains(criteria.LastName, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(criteria.Email))
            {
                members = members.Where(m => m.Email.Contains(criteria.Email, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(criteria.Phone))
            {
                members = members.Where(m => m.Phone.Contains(criteria.Phone, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(criteria.Gender))
            {
                members = members.Where(m => m.Gender.Equals(criteria.Gender, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(criteria.Address))
            {
                members = members.Where(m => m.Address.Contains(criteria.Address, StringComparison.OrdinalIgnoreCase));
            }

            if (criteria.DateOfBirthFrom.HasValue)
            {
                members = members.Where(m => m.DOB >= criteria.DateOfBirthFrom.Value);
            }

            if (criteria.DateOfBirthTo.HasValue)
            {
                members = members.Where(m => m.DOB <= criteria.DateOfBirthTo.Value);
            }

            if (criteria.AgeFrom.HasValue)
            {
                var maxDateForAgeFrom = DateTime.Today.AddYears(-criteria.AgeFrom.Value);
                members = members.Where(m => m.DOB <= maxDateForAgeFrom);
            }

            if (criteria.AgeTo.HasValue)
            {
                var minDateForAgeTo = DateTime.Today.AddYears(-criteria.AgeTo.Value - 1);
                members = members.Where(m => m.DOB > minDateForAgeTo);
            }

            return members;
        }

        private IEnumerable<Member> ApplySorting(IEnumerable<Member> members, MemberSearchCriteriaDto criteria)
        {
            if (string.IsNullOrEmpty(criteria.SortBy))
                criteria.SortBy = "Id";

            var sortedMembers = criteria.SortBy.ToLower() switch
            {
                "membernumber" => criteria.SortDescending 
                    ? members.OrderByDescending(m => m.MemberNumber)
                    : members.OrderBy(m => m.MemberNumber),
                "firstname" => criteria.SortDescending
                    ? members.OrderByDescending(m => m.FirstName)
                    : members.OrderBy(m => m.FirstName),
                "lastname" => criteria.SortDescending
                    ? members.OrderByDescending(m => m.LastName)
                    : members.OrderBy(m => m.LastName),
                "dob" => criteria.SortDescending
                    ? members.OrderByDescending(m => m.DOB)
                    : members.OrderBy(m => m.DOB),
                "gender" => criteria.SortDescending
                    ? members.OrderByDescending(m => m.Gender)
                    : members.OrderBy(m => m.Gender),
                "email" => criteria.SortDescending
                    ? members.OrderByDescending(m => m.Email)
                    : members.OrderBy(m => m.Email),
                "phone" => criteria.SortDescending
                    ? members.OrderByDescending(m => m.Phone)
                    : members.OrderBy(m => m.Phone),
                "address" => criteria.SortDescending
                    ? members.OrderByDescending(m => m.Address)
                    : members.OrderBy(m => m.Address),
                _ => criteria.SortDescending
                    ? members.OrderByDescending(m => m.Id)
                    : members.OrderBy(m => m.Id)
            };

            return sortedMembers;
        }

        public async Task<MemberDto> CreateMemberAsync(CreateMemberDto createMemberDto)
        {
            // Check if member number already exists
            if (await _unitOfWork.Members.IsMemberNumberExistsAsync(createMemberDto.MemberNumber))
            {
                throw new InvalidOperationException($"Member number '{createMemberDto.MemberNumber}' already exists.");
            }

            var member = new Member
            {
                MemberNumber = createMemberDto.MemberNumber,
                FirstName = createMemberDto.FirstName,
                LastName = createMemberDto.LastName,
                DOB = createMemberDto.DOB,
                Gender = createMemberDto.Gender,
                Address = createMemberDto.Address,
                Phone = createMemberDto.Phone,
                Email = createMemberDto.Email,
                RxClaims = new List<Domain.Entities.RxClaims.RxClaim>(),
                PriorAuthorizations = new List<Domain.Entities.PriorAuthorization.PriorAuthorizationRecord>()
            };

            await _unitOfWork.Members.AddAsync(member);
            await _unitOfWork.CompleteAsync();

            return MapToDto(member);
        }

        public async Task<MemberDto?> UpdateMemberAsync(int id, UpdateMemberDto updateMemberDto)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(id);
            if (member == null)
                return null;

            member.FirstName = updateMemberDto.FirstName;
            member.LastName = updateMemberDto.LastName;
            member.DOB = updateMemberDto.DOB;
            member.Gender = updateMemberDto.Gender;
            member.Address = updateMemberDto.Address;
            member.Phone = updateMemberDto.Phone;
            member.Email = updateMemberDto.Email;

            _unitOfWork.Members.Update(member);
            await _unitOfWork.CompleteAsync();

            return MapToDto(member);
        }

        public async Task<bool> DeleteMemberAsync(int id)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(id);
            if (member == null)
                return false;

            _unitOfWork.Members.Remove(member);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> IsMemberNumberExistsAsync(string memberNumber)
        {
            return await _unitOfWork.Members.IsMemberNumberExistsAsync(memberNumber);
        }

        private static MemberDto MapToDto(Member member)
        {
            return new MemberDto
            {
                Id = member.Id,
                MemberNumber = member.MemberNumber,
                FirstName = member.FirstName,
                LastName = member.LastName,
                DOB = member.DOB,
                Gender = member.Gender,
                Address = member.Address,
                Phone = member.Phone,
                Email = member.Email
            };
        }
    }
}