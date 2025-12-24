using Pharmacy.Application.DTOs.Providers;

namespace Pharmacy.Application.Services.Interfaces
{
    public interface IProviderService
    {
        Task<IEnumerable<ProviderDto>> GetAllProvidersAsync();
        Task<ProviderDto?> GetProviderByIdAsync(int id);
        Task<ProviderDto?> GetProviderByProviderNumberAsync(string providerNumber);
        Task<ProviderDto?> GetProviderByNPIAsync(string npi);
        Task<IEnumerable<ProviderDto>> GetProvidersBySpecialtyAsync(string specialty);
        Task<IEnumerable<ProviderDto>> SearchProvidersAsync(string searchTerm);
        Task<ProviderSearchResultDto> AdvancedSearchProvidersAsync(ProviderSearchCriteriaDto searchCriteria);
        Task<ProviderDto> CreateProviderAsync(CreateProviderDto createProviderDto);
        Task<ProviderDto?> UpdateProviderAsync(int id, UpdateProviderDto updateProviderDto);
        Task<bool> DeleteProviderAsync(int id);
        Task<bool> IsProviderNumberExistsAsync(string providerNumber);
        Task<bool> IsNPIExistsAsync(string npi);
    }
}