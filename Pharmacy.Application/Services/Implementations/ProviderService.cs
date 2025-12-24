using Pharmacy.Application.DTOs.Providers;
using Pharmacy.Application.Services.Interfaces;
using Pharmacy.Domain.Entities.Providers;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Services.Implementations
{
    public class ProviderService : IProviderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProviderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProviderDto>> GetAllProvidersAsync()
        {
            var providers = await _unitOfWork.Providers.GetAllAsync();
            return providers.Select(MapToDto);
        }

        public async Task<ProviderDto?> GetProviderByIdAsync(int id)
        {
            var provider = await _unitOfWork.Providers.GetByIdAsync(id);
            return provider != null ? MapToDto(provider) : null;
        }

        public async Task<ProviderDto?> GetProviderByProviderNumberAsync(string providerNumber)
        {
            var provider = await _unitOfWork.Providers.GetByProviderNumberAsync(providerNumber);
            return provider != null ? MapToDto(provider) : null;
        }

        public async Task<ProviderDto?> GetProviderByNPIAsync(string npi)
        {
            var provider = await _unitOfWork.Providers.GetByNPIAsync(npi);
            return provider != null ? MapToDto(provider) : null;
        }

        public async Task<IEnumerable<ProviderDto>> GetProvidersBySpecialtyAsync(string specialty)
        {
            var providers = await _unitOfWork.Providers.GetProvidersBySpecialtyAsync(specialty);
            return providers.Select(MapToDto);
        }

        public async Task<IEnumerable<ProviderDto>> SearchProvidersAsync(string searchTerm)
        {
            var providers = await _unitOfWork.Providers.SearchProvidersAsync(searchTerm);
            return providers.Select(MapToDto);
        }

        public async Task<ProviderSearchResultDto> AdvancedSearchProvidersAsync(ProviderSearchCriteriaDto searchCriteria)
        {
            var (providers, totalCount) = await _unitOfWork.Providers.AdvancedSearchAsync(
                providerNumber: searchCriteria.ProviderNumber,
                name: searchCriteria.Name,
                npi: searchCriteria.NPI,
                email: searchCriteria.Email,
                phone: searchCriteria.Phone,
                specialty: searchCriteria.Specialty,
                address: searchCriteria.Address,
                sortBy: searchCriteria.SortBy ?? "Id",
                sortDescending: searchCriteria.SortDescending,
                pageNumber: searchCriteria.PageNumber,
                pageSize: searchCriteria.PageSize);

            var totalPages = (int)Math.Ceiling(totalCount / (double)searchCriteria.PageSize);

            return new ProviderSearchResultDto
            {
                Providers = providers.Select(MapToDto),
                TotalCount = totalCount,
                PageNumber = searchCriteria.PageNumber,
                PageSize = searchCriteria.PageSize,
                TotalPages = totalPages,
                HasPreviousPage = searchCriteria.PageNumber > 1,
                HasNextPage = searchCriteria.PageNumber < totalPages
            };
        }

        public async Task<ProviderDto> CreateProviderAsync(CreateProviderDto createProviderDto)
        {
            // Check if provider number already exists
            if (await _unitOfWork.Providers.IsProviderNumberExistsAsync(createProviderDto.ProviderNumber))
            {
                throw new InvalidOperationException($"Provider number '{createProviderDto.ProviderNumber}' already exists.");
            }

            // Check if NPI already exists
            if (await _unitOfWork.Providers.IsNPIExistsAsync(createProviderDto.NPI))
            {
                throw new InvalidOperationException($"NPI '{createProviderDto.NPI}' already exists.");
            }

            var provider = new Provider
            {
                ProviderNumber = createProviderDto.ProviderNumber,
                Name = createProviderDto.Name,
                NPI = createProviderDto.NPI,
                Address = createProviderDto.Address,
                Phone = createProviderDto.Phone,
                Email = createProviderDto.Email,
                Specialty = createProviderDto.Specialty,
                RxClaims = new List<Domain.Entities.RxClaims.RxClaim>(),
                PriorAuthorizations = new List<Domain.Entities.PriorAuthorization.PriorAuthorizationRecord>()
            };

            await _unitOfWork.Providers.AddAsync(provider);
            await _unitOfWork.CompleteAsync();

            return MapToDto(provider);
        }

        public async Task<ProviderDto?> UpdateProviderAsync(int id, UpdateProviderDto updateProviderDto)
        {
            var provider = await _unitOfWork.Providers.GetByIdAsync(id);
            if (provider == null)
                return null;

            // Check if NPI is being changed and if the new NPI already exists
            if (provider.NPI != updateProviderDto.NPI)
            {
                if (await _unitOfWork.Providers.IsNPIExistsAsync(updateProviderDto.NPI))
                {
                    throw new InvalidOperationException($"NPI '{updateProviderDto.NPI}' already exists.");
                }
            }

            provider.Name = updateProviderDto.Name;
            provider.NPI = updateProviderDto.NPI;
            provider.Address = updateProviderDto.Address;
            provider.Phone = updateProviderDto.Phone;
            provider.Email = updateProviderDto.Email;
            provider.Specialty = updateProviderDto.Specialty;

            _unitOfWork.Providers.Update(provider);
            await _unitOfWork.CompleteAsync();

            return MapToDto(provider);
        }

        public async Task<bool> DeleteProviderAsync(int id)
        {
            var provider = await _unitOfWork.Providers.GetByIdAsync(id);
            if (provider == null)
                return false;

            _unitOfWork.Providers.Remove(provider);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> IsProviderNumberExistsAsync(string providerNumber)
        {
            return await _unitOfWork.Providers.IsProviderNumberExistsAsync(providerNumber);
        }

        public async Task<bool> IsNPIExistsAsync(string npi)
        {
            return await _unitOfWork.Providers.IsNPIExistsAsync(npi);
        }

        private static ProviderDto MapToDto(Provider provider)
        {
            return new ProviderDto
            {
                Id = provider.Id,
                ProviderNumber = provider.ProviderNumber,
                Name = provider.Name,
                NPI = provider.NPI,
                Address = provider.Address,
                Phone = provider.Phone,
                Email = provider.Email,
                Specialty = provider.Specialty
            };
        }
    }
}