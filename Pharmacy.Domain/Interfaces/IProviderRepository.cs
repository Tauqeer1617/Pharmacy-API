using Pharmacy.Domain.Entities.Providers;

namespace Pharmacy.Domain.Interfaces
{
    public interface IProviderRepository : IGenericRepository<Provider>
    {
        Task<Provider?> GetByProviderNumberAsync(string providerNumber);
        Task<Provider?> GetByNPIAsync(string npi);
        Task<IEnumerable<Provider>> GetProvidersBySpecialtyAsync(string specialty);
        Task<IEnumerable<Provider>> SearchProvidersAsync(string searchTerm);
        Task<bool> IsProviderNumberExistsAsync(string providerNumber);
        Task<bool> IsNPIExistsAsync(string npi);
        Task<(IEnumerable<Provider> Providers, int TotalCount)> AdvancedSearchAsync(
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
            int pageSize = 10);
    }
}