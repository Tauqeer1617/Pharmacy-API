using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities.Providers;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Data;

namespace Pharmacy.Infrastructure.Repositories
{
    public class ProviderRepository : GenericRepository<Provider>, IProviderRepository
    {
        public ProviderRepository(PharmacyDbContext context) : base(context)
        {
        }

        public async Task<Provider?> GetByProviderNumberAsync(string providerNumber)
        {
            return await _dbSet
                .Include(p => p.RxClaims)
                .Include(p => p.PriorAuthorizations)
                .FirstOrDefaultAsync(p => p.ProviderNumber == providerNumber);
        }

        public async Task<Provider?> GetByNPIAsync(string npi)
        {
            return await _dbSet
                .Include(p => p.RxClaims)
                .Include(p => p.PriorAuthorizations)
                .FirstOrDefaultAsync(p => p.NPI == npi);
        }

        public async Task<IEnumerable<Provider>> GetProvidersBySpecialtyAsync(string specialty)
        {
            return await _dbSet
                .Where(p => p.Specialty.ToLower() == specialty.ToLower())
                .ToListAsync();
        }

        public async Task<IEnumerable<Provider>> SearchProvidersAsync(string searchTerm)
        {
            return await _dbSet
                .Where(p => p.Name.Contains(searchTerm) ||
                           p.ProviderNumber.Contains(searchTerm) ||
                           p.NPI.Contains(searchTerm) ||
                           p.Email.Contains(searchTerm) ||
                           p.Specialty.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<bool> IsProviderNumberExistsAsync(string providerNumber)
        {
            return await _dbSet.AnyAsync(p => p.ProviderNumber == providerNumber);
        }

        public async Task<bool> IsNPIExistsAsync(string npi)
        {
            return await _dbSet.AnyAsync(p => p.NPI == npi);
        }

        public async Task<(IEnumerable<Provider> Providers, int TotalCount)> AdvancedSearchAsync(
            string? providerNumber = null,
            string? name = null,
            string? npi = null,
            string? email = null,
            string? phone = null,
            string? specialty = null,
            string? address = null,
            string sortBy = "Id",
            bool sortDescending = false,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = _dbSet.AsQueryable();

            // Apply filters with null-safe and case-insensitive comparisons
            if (!string.IsNullOrWhiteSpace(providerNumber))
            {
                query = query.Where(p => EF.Functions.Like(p.ProviderNumber.ToLower(), $"%{providerNumber.ToLower()}%"));
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(p => EF.Functions.Like(p.Name.ToLower(), $"%{name.ToLower()}%"));
            }

            if (!string.IsNullOrWhiteSpace(npi))
            {
                query = query.Where(p => EF.Functions.Like(p.NPI.ToLower(), $"%{npi.ToLower()}%"));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(p => EF.Functions.Like(p.Email.ToLower(), $"%{email.ToLower()}%"));
            }

            if (!string.IsNullOrWhiteSpace(phone))
            {
                query = query.Where(p => EF.Functions.Like(p.Phone.ToLower(), $"%{phone.ToLower()}%"));
            }

            if (!string.IsNullOrWhiteSpace(specialty))
            {
                query = query.Where(p => EF.Functions.Like(p.Specialty.ToLower(), $"%{specialty.ToLower()}%"));
            }

            if (!string.IsNullOrWhiteSpace(address))
            {
                query = query.Where(p => EF.Functions.Like(p.Address.ToLower(), $"%{address.ToLower()}%"));
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            query = ApplySorting(query, sortBy, sortDescending);

            // Apply pagination
            var providers = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (providers, totalCount);
        }

        private IQueryable<Provider> ApplySorting(IQueryable<Provider> query, string sortBy, bool sortDescending)
        {
            return sortBy.ToLower() switch
            {
                "providernumber" => sortDescending 
                    ? query.OrderByDescending(p => p.ProviderNumber)
                    : query.OrderBy(p => p.ProviderNumber),
                "name" => sortDescending
                    ? query.OrderByDescending(p => p.Name)
                    : query.OrderBy(p => p.Name),
                "npi" => sortDescending
                    ? query.OrderByDescending(p => p.NPI)
                    : query.OrderBy(p => p.NPI),
                "email" => sortDescending
                    ? query.OrderByDescending(p => p.Email)
                    : query.OrderBy(p => p.Email),
                "phone" => sortDescending
                    ? query.OrderByDescending(p => p.Phone)
                    : query.OrderBy(p => p.Phone),
                "specialty" => sortDescending
                    ? query.OrderByDescending(p => p.Specialty)
                    : query.OrderBy(p => p.Specialty),
                "address" => sortDescending
                    ? query.OrderByDescending(p => p.Address)
                    : query.OrderBy(p => p.Address),
                _ => sortDescending
                    ? query.OrderByDescending(p => p.Id)
                    : query.OrderBy(p => p.Id)
            };
        }
    }
}