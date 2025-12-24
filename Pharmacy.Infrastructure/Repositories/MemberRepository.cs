using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities.Members;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Data;

namespace Pharmacy.Infrastructure.Repositories
{
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        public MemberRepository(PharmacyDbContext context) : base(context)
        {
        }

        public async Task<Member?> GetByMemberNumberAsync(string memberNumber)
        {
            return await _dbSet
                .Include(m => m.RxClaims)
                .Include(m => m.PriorAuthorizations)
                .FirstOrDefaultAsync(m => m.MemberNumber == memberNumber);
        }

        public async Task<IEnumerable<Member>> GetMembersByGenderAsync(string gender)
        {
            return await _dbSet
                .Where(m => m.Gender.ToLower() == gender.ToLower())
                .ToListAsync();
        }

        public async Task<IEnumerable<Member>> SearchMembersAsync(string searchTerm)
        {
            return await _dbSet
                .Where(m => m.FirstName.Contains(searchTerm) ||
                           m.LastName.Contains(searchTerm) ||
                           m.MemberNumber.Contains(searchTerm) ||
                           m.Email.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<bool> IsMemberNumberExistsAsync(string memberNumber)
        {
            return await _dbSet.AnyAsync(m => m.MemberNumber == memberNumber);
        }

        public async Task<(IEnumerable<Member> Members, int TotalCount)> AdvancedSearchAsync(
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
            int pageSize = 10)
        {
            var query = _dbSet.AsQueryable();

            // Apply filters with null-safe and case-insensitive comparisons
            if (!string.IsNullOrWhiteSpace(memberNumber))
            {
                query = query.Where(m => EF.Functions.Like(m.MemberNumber.ToLower(), $"%{memberNumber.ToLower()}%"));
            }

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                query = query.Where(m => EF.Functions.Like(m.FirstName.ToLower(), $"%{firstName.ToLower()}%"));
            }

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                query = query.Where(m => EF.Functions.Like(m.LastName.ToLower(), $"%{lastName.ToLower()}%"));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(m => EF.Functions.Like(m.Email.ToLower(), $"%{email.ToLower()}%"));
            }

            if (!string.IsNullOrWhiteSpace(phone))
            {
                query = query.Where(m => EF.Functions.Like(m.Phone.ToLower(), $"%{phone.ToLower()}%"));
            }

            if (!string.IsNullOrWhiteSpace(gender))
            {
                query = query.Where(m => EF.Functions.Like(m.Gender.ToLower(), gender.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(address))
            {
                query = query.Where(m => EF.Functions.Like(m.Address.ToLower(), $"%{address.ToLower()}%"));
            }

            if (dateOfBirthFrom.HasValue)
            {
                query = query.Where(m => m.DOB >= dateOfBirthFrom.Value);
            }

            if (dateOfBirthTo.HasValue)
            {
                query = query.Where(m => m.DOB <= dateOfBirthTo.Value);
            }

            // Age filtering - convert to date ranges
            if (ageFrom.HasValue)
            {
                var maxDateForAgeFrom = DateTime.Today.AddYears(-ageFrom.Value);
                query = query.Where(m => m.DOB <= maxDateForAgeFrom);
            }

            if (ageTo.HasValue)
            {
                var minDateForAgeTo = DateTime.Today.AddYears(-ageTo.Value - 1);
                query = query.Where(m => m.DOB > minDateForAgeTo);
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            query = ApplySorting(query, sortBy, sortDescending);

            // Apply pagination
            var members = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (members, totalCount);
        }

        private IQueryable<Member> ApplySorting(IQueryable<Member> query, string sortBy, bool sortDescending)
        {
            return sortBy.ToLower() switch
            {
                "membernumber" => sortDescending 
                    ? query.OrderByDescending(m => m.MemberNumber)
                    : query.OrderBy(m => m.MemberNumber),
                "firstname" => sortDescending
                    ? query.OrderByDescending(m => m.FirstName)
                    : query.OrderBy(m => m.FirstName),
                "lastname" => sortDescending
                    ? query.OrderByDescending(m => m.LastName)
                    : query.OrderBy(m => m.LastName),
                "dob" => sortDescending
                    ? query.OrderByDescending(m => m.DOB)
                    : query.OrderBy(m => m.DOB),
                "gender" => sortDescending
                    ? query.OrderByDescending(m => m.Gender)
                    : query.OrderBy(m => m.Gender),
                "email" => sortDescending
                    ? query.OrderByDescending(m => m.Email)
                    : query.OrderBy(m => m.Email),
                "phone" => sortDescending
                    ? query.OrderByDescending(m => m.Phone)
                    : query.OrderBy(m => m.Phone),
                "address" => sortDescending
                    ? query.OrderByDescending(m => m.Address)
                    : query.OrderBy(m => m.Address),
                _ => sortDescending
                    ? query.OrderByDescending(m => m.Id)
                    : query.OrderBy(m => m.Id)
            };
        }
    }
}