using Moq;
using Pharmacy.Application.DTOs.Providers;
using Pharmacy.Application.Services.Implementations;
using Pharmacy.Domain.Entities.Providers;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Tests.TestData;

namespace Pharmacy.Tests.Services
{
    public class ProviderServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IProviderRepository> _mockProviderRepository;
        private readonly ProviderService _providerService;

        public ProviderServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProviderRepository = new Mock<IProviderRepository>();
            _mockUnitOfWork.Setup(x => x.Providers).Returns(_mockProviderRepository.Object);
            _providerService = new ProviderService(_mockUnitOfWork.Object);
        }

        #region GetAllProvidersAsync Tests

        [Fact]
        public async Task GetAllProvidersAsync_ReturnsAllProviders_WhenProvidersExist()
        {
            // Arrange
            var providers = new List<Provider>
            {
                CreateTestProvider(1, "PROV001", "Dr. John Doe", "1234567890", "Family Medicine"),
                CreateTestProvider(2, "PROV002", "Dr. Jane Smith", "2345678901", "Cardiology")
            };

            _mockProviderRepository.Setup(x => x.GetAllAsync())
                .ReturnsAsync(providers);

            // Act
            var result = await _providerService.GetAllProvidersAsync();

            // Assert
            Assert.NotNull(result);
            var providerList = result.ToList();
            Assert.Equal(2, providerList.Count);
            Assert.Equal("Dr. John Doe", providerList[0].Name);
            Assert.Equal("Dr. Jane Smith", providerList[1].Name);
        }

        [Fact]
        public async Task GetAllProvidersAsync_ReturnsEmptyList_WhenNoProvidersExist()
        {
            // Arrange
            _mockProviderRepository.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Provider>());

            // Act
            var result = await _providerService.GetAllProvidersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region GetProviderByIdAsync Tests

        [Fact]
        public async Task GetProviderByIdAsync_ReturnsProvider_WhenProviderExists()
        {
            // Arrange
            var providerId = 1;
            var provider = CreateTestProvider(providerId, "PROV001", "Dr. John Doe", "1234567890", "Family Medicine");

            _mockProviderRepository.Setup(x => x.GetByIdAsync(providerId))
                .ReturnsAsync(provider);

            // Act
            var result = await _providerService.GetProviderByIdAsync(providerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(providerId, result.Id);
            Assert.Equal("Dr. John Doe", result.Name);
            Assert.Equal("PROV001", result.ProviderNumber);
        }

        [Fact]
        public async Task GetProviderByIdAsync_ReturnsNull_WhenProviderDoesNotExist()
        {
            // Arrange
            var providerId = 999;
            _mockProviderRepository.Setup(x => x.GetByIdAsync(providerId))
                .ReturnsAsync((Provider?)null);

            // Act
            var result = await _providerService.GetProviderByIdAsync(providerId);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region GetProviderByProviderNumberAsync Tests

        [Fact]
        public async Task GetProviderByProviderNumberAsync_ReturnsProvider_WhenProviderExists()
        {
            // Arrange
            var providerNumber = "PROV001";
            var provider = CreateTestProvider(1, providerNumber, "Dr. John Doe", "1234567890", "Family Medicine");

            _mockProviderRepository.Setup(x => x.GetByProviderNumberAsync(providerNumber))
                .ReturnsAsync(provider);

            // Act
            var result = await _providerService.GetProviderByProviderNumberAsync(providerNumber);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(providerNumber, result.ProviderNumber);
            Assert.Equal("Dr. John Doe", result.Name);
        }

        [Fact]
        public async Task GetProviderByProviderNumberAsync_ReturnsNull_WhenProviderDoesNotExist()
        {
            // Arrange
            var providerNumber = "NONEXISTENT";
            _mockProviderRepository.Setup(x => x.GetByProviderNumberAsync(providerNumber))
                .ReturnsAsync((Provider?)null);

            // Act
            var result = await _providerService.GetProviderByProviderNumberAsync(providerNumber);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region GetProviderByNPIAsync Tests

        [Fact]
        public async Task GetProviderByNPIAsync_ReturnsProvider_WhenProviderExists()
        {
            // Arrange
            var npi = "1234567890";
            var provider = CreateTestProvider(1, "PROV001", "Dr. John Doe", npi, "Family Medicine");

            _mockProviderRepository.Setup(x => x.GetByNPIAsync(npi))
                .ReturnsAsync(provider);

            // Act
            var result = await _providerService.GetProviderByNPIAsync(npi);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(npi, result.NPI);
            Assert.Equal("Dr. John Doe", result.Name);
        }

        [Fact]
        public async Task GetProviderByNPIAsync_ReturnsNull_WhenProviderDoesNotExist()
        {
            // Arrange
            var npi = "9999999999";
            _mockProviderRepository.Setup(x => x.GetByNPIAsync(npi))
                .ReturnsAsync((Provider?)null);

            // Act
            var result = await _providerService.GetProviderByNPIAsync(npi);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region GetProvidersBySpecialtyAsync Tests

        [Fact]
        public async Task GetProvidersBySpecialtyAsync_ReturnsFilteredProviders_WhenSpecialtyMatches()
        {
            // Arrange
            var specialty = "Cardiology";
            var providers = new List<Provider>
            {
                CreateTestProvider(1, "PROV001", "Dr. Heart Specialist", "1234567890", specialty),
                CreateTestProvider(2, "PROV002", "Dr. Another Heart Doc", "2345678901", specialty)
            };

            _mockProviderRepository.Setup(x => x.GetProvidersBySpecialtyAsync(specialty))
                .ReturnsAsync(providers);

            // Act
            var result = await _providerService.GetProvidersBySpecialtyAsync(specialty);

            // Assert
            Assert.NotNull(result);
            var providerList = result.ToList();
            Assert.Equal(2, providerList.Count);
            Assert.All(providerList, p => Assert.Equal(specialty, p.Specialty));
        }

        [Fact]
        public async Task GetProvidersBySpecialtyAsync_ReturnsEmptyList_WhenNoProvidersMatchSpecialty()
        {
            // Arrange
            var specialty = "Nonexistent Specialty";
            _mockProviderRepository.Setup(x => x.GetProvidersBySpecialtyAsync(specialty))
                .ReturnsAsync(new List<Provider>());

            // Act
            var result = await _providerService.GetProvidersBySpecialtyAsync(specialty);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region SearchProvidersAsync Tests

        [Fact]
        public async Task SearchProvidersAsync_ReturnsMatchingProviders_WhenSearchTermMatches()
        {
            // Arrange
            var searchTerm = "Kumar";
            var providers = new List<Provider>
            {
                CreateTestProvider(1, "PROV001", "Dr. David Kumar", "1234567890", "Orthopedics")
            };

            _mockProviderRepository.Setup(x => x.SearchProvidersAsync(searchTerm))
                .ReturnsAsync(providers);

            // Act
            var result = await _providerService.SearchProvidersAsync(searchTerm);

            // Assert
            Assert.NotNull(result);
            var providerList = result.ToList();
            Assert.Single(providerList);
            Assert.Contains("Kumar", providerList[0].Name);
        }

        [Fact]
        public async Task SearchProvidersAsync_ReturnsEmptyList_WhenNoProvidersMatch()
        {
            // Arrange
            var searchTerm = "NonexistentName";
            _mockProviderRepository.Setup(x => x.SearchProvidersAsync(searchTerm))
                .ReturnsAsync(new List<Provider>());

            // Act
            var result = await _providerService.SearchProvidersAsync(searchTerm);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region AdvancedSearchProvidersAsync Tests

        [Fact]
        public async Task AdvancedSearchProvidersAsync_ReturnsSearchResult_WithCorrectPagination()
        {
            // Arrange
            var searchCriteria = new ProviderSearchCriteriaDto
            {
                Name = "Dr.",
                PageNumber = 1,
                PageSize = 10,
                SortBy = "Name",
                SortDescending = false
            };

            var providers = new List<Provider>
            {
                CreateTestProvider(1, "PROV001", "Dr. John Doe", "1234567890", "Family Medicine"),
                CreateTestProvider(2, "PROV002", "Dr. Jane Smith", "2345678901", "Cardiology")
            };

            _mockProviderRepository.Setup(x => x.AdvancedSearchAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((providers, 15)); // Total count of 15

            // Act
            var result = await _providerService.AdvancedSearchProvidersAsync(searchCriteria);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(15, result.TotalCount);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(10, result.PageSize);
            Assert.Equal(2, result.TotalPages); // 15 / 10 = 2
            Assert.False(result.HasPreviousPage);
            Assert.True(result.HasNextPage);
            Assert.Equal(2, result.Providers.Count());
        }

        [Fact]
        public async Task AdvancedSearchProvidersAsync_HandlesEmptyResults()
        {
            // Arrange
            var searchCriteria = new ProviderSearchCriteriaDto
            {
                Name = "NonexistentName",
                PageNumber = 1,
                PageSize = 10
            };

            _mockProviderRepository.Setup(x => x.AdvancedSearchAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((new List<Provider>(), 0));

            // Act
            var result = await _providerService.AdvancedSearchProvidersAsync(searchCriteria);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.TotalCount);
            Assert.Equal(0, result.TotalPages);
            Assert.False(result.HasPreviousPage);
            Assert.False(result.HasNextPage);
            Assert.Empty(result.Providers);
        }

        [Fact]
        public async Task AdvancedSearchProvidersAsync_UsesDefaultSortBy_WhenSortByIsNull()
        {
            // Arrange
            var searchCriteria = new ProviderSearchCriteriaDto
            {
                PageNumber = 1,
                PageSize = 10,
                SortBy = null // Test default value
            };

            _mockProviderRepository.Setup(x => x.AdvancedSearchAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), "Id", It.IsAny<bool>(),
                It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((new List<Provider>(), 0));

            // Act
            await _providerService.AdvancedSearchProvidersAsync(searchCriteria);

            // Assert
            _mockProviderRepository.Verify(x => x.AdvancedSearchAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), "Id", It.IsAny<bool>(),
                It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        #endregion

        #region CreateProviderAsync Tests

        [Fact]
        public async Task CreateProviderAsync_CreatesProvider_WhenValidDataProvided()
        {
            // Arrange
            var createDto = ProviderTestDataBuilder.GetSingleTestProvider();

            _mockProviderRepository.Setup(x => x.IsProviderNumberExistsAsync(createDto.ProviderNumber))
                .ReturnsAsync(false);
            _mockProviderRepository.Setup(x => x.IsNPIExistsAsync(createDto.NPI))
                .ReturnsAsync(false);
            _mockProviderRepository.Setup(x => x.AddAsync(It.IsAny<Provider>()))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(x => x.CompleteAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _providerService.CreateProviderAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createDto.ProviderNumber, result.ProviderNumber);
            Assert.Equal(createDto.Name, result.Name);
            Assert.Equal(createDto.NPI, result.NPI);

            _mockProviderRepository.Verify(x => x.AddAsync(It.IsAny<Provider>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateProviderAsync_ThrowsException_WhenProviderNumberAlreadyExists()
        {
            // Arrange
            var createDto = ProviderTestDataBuilder.GetSingleTestProvider();

            _mockProviderRepository.Setup(x => x.IsProviderNumberExistsAsync(createDto.ProviderNumber))
                .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _providerService.CreateProviderAsync(createDto));

            Assert.Contains("Provider number", exception.Message);
            Assert.Contains("already exists", exception.Message);

            _mockProviderRepository.Verify(x => x.AddAsync(It.IsAny<Provider>()), Times.Never);
            _mockUnitOfWork.Verify(x => x.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateProviderAsync_ThrowsException_WhenNPIAlreadyExists()
        {
            // Arrange
            var createDto = ProviderTestDataBuilder.GetSingleTestProvider();

            _mockProviderRepository.Setup(x => x.IsProviderNumberExistsAsync(createDto.ProviderNumber))
                .ReturnsAsync(false);
            _mockProviderRepository.Setup(x => x.IsNPIExistsAsync(createDto.NPI))
                .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _providerService.CreateProviderAsync(createDto));

            Assert.Contains("NPI", exception.Message);
            Assert.Contains("already exists", exception.Message);

            _mockProviderRepository.Verify(x => x.AddAsync(It.IsAny<Provider>()), Times.Never);
            _mockUnitOfWork.Verify(x => x.CompleteAsync(), Times.Never);
        }

        #endregion

        #region UpdateProviderAsync Tests

        [Fact]
        public async Task UpdateProviderAsync_UpdatesProvider_WhenValidDataProvided()
        {
            // Arrange
            var providerId = 1;
            var existingProvider = CreateTestProvider(providerId, "PROV001", "Dr. Old Name", "1234567890", "Old Specialty");
            var updateDto = ProviderTestDataBuilder.GetValidUpdateData();

            _mockProviderRepository.Setup(x => x.GetByIdAsync(providerId))
                .ReturnsAsync(existingProvider);
            _mockProviderRepository.Setup(x => x.IsNPIExistsAsync(updateDto.NPI))
                .ReturnsAsync(false);
            _mockProviderRepository.Setup(x => x.Update(It.IsAny<Provider>()));
            _mockUnitOfWork.Setup(x => x.CompleteAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _providerService.UpdateProviderAsync(providerId, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(providerId, result.Id);
            Assert.Equal(updateDto.Name, result.Name);
            Assert.Equal(updateDto.NPI, result.NPI);
            Assert.Equal(updateDto.Specialty, result.Specialty);

            _mockProviderRepository.Verify(x => x.Update(It.IsAny<Provider>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateProviderAsync_ReturnsNull_WhenProviderDoesNotExist()
        {
            // Arrange
            var providerId = 999;
            var updateDto = ProviderTestDataBuilder.GetValidUpdateData();

            _mockProviderRepository.Setup(x => x.GetByIdAsync(providerId))
                .ReturnsAsync((Provider?)null);

            // Act
            var result = await _providerService.UpdateProviderAsync(providerId, updateDto);

            // Assert
            Assert.Null(result);

            _mockProviderRepository.Verify(x => x.Update(It.IsAny<Provider>()), Times.Never);
            _mockUnitOfWork.Verify(x => x.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateProviderAsync_ThrowsException_WhenNPIChangedToExistingNPI()
        {
            // Arrange
            var providerId = 1;
            var existingProvider = CreateTestProvider(providerId, "PROV001", "Dr. John", "1234567890", "Specialty");
            var updateDto = ProviderTestDataBuilder.GetValidUpdateData();
            updateDto.NPI = "9999999999"; // Different NPI

            _mockProviderRepository.Setup(x => x.GetByIdAsync(providerId))
                .ReturnsAsync(existingProvider);
            _mockProviderRepository.Setup(x => x.IsNPIExistsAsync(updateDto.NPI))
                .ReturnsAsync(true); // NPI already exists

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _providerService.UpdateProviderAsync(providerId, updateDto));

            Assert.Contains("NPI", exception.Message);
            Assert.Contains("already exists", exception.Message);

            _mockProviderRepository.Verify(x => x.Update(It.IsAny<Provider>()), Times.Never);
            _mockUnitOfWork.Verify(x => x.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateProviderAsync_DoesNotCheckNPI_WhenNPINotChanged()
        {
            // Arrange
            var providerId = 1;
            var existingNPI = "1234567890";
            var existingProvider = CreateTestProvider(providerId, "PROV001", "Dr. John", existingNPI, "Specialty");
            var updateDto = ProviderTestDataBuilder.GetValidUpdateData();
            updateDto.NPI = existingNPI; // Same NPI

            _mockProviderRepository.Setup(x => x.GetByIdAsync(providerId))
                .ReturnsAsync(existingProvider);
            _mockProviderRepository.Setup(x => x.Update(It.IsAny<Provider>()));
            _mockUnitOfWork.Setup(x => x.CompleteAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _providerService.UpdateProviderAsync(providerId, updateDto);

            // Assert
            Assert.NotNull(result);

            // Verify that NPI existence was not checked since it wasn't changed
            _mockProviderRepository.Verify(x => x.IsNPIExistsAsync(It.IsAny<string>()), Times.Never);
        }

        #endregion

        #region DeleteProviderAsync Tests

        [Fact]
        public async Task DeleteProviderAsync_ReturnsTrue_WhenProviderExistsAndIsDeleted()
        {
            // Arrange
            var providerId = 1;
            var provider = CreateTestProvider(providerId, "PROV001", "Dr. John Doe", "1234567890", "Family Medicine");

            _mockProviderRepository.Setup(x => x.GetByIdAsync(providerId))
                .ReturnsAsync(provider);
            _mockProviderRepository.Setup(x => x.Remove(provider));
            _mockUnitOfWork.Setup(x => x.CompleteAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _providerService.DeleteProviderAsync(providerId);

            // Assert
            Assert.True(result);

            _mockProviderRepository.Verify(x => x.Remove(provider), Times.Once);
            _mockUnitOfWork.Verify(x => x.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteProviderAsync_ReturnsFalse_WhenProviderDoesNotExist()
        {
            // Arrange
            var providerId = 999;

            _mockProviderRepository.Setup(x => x.GetByIdAsync(providerId))
                .ReturnsAsync((Provider?)null);

            // Act
            var result = await _providerService.DeleteProviderAsync(providerId);

            // Assert
            Assert.False(result);

            _mockProviderRepository.Verify(x => x.Remove(It.IsAny<Provider>()), Times.Never);
            _mockUnitOfWork.Verify(x => x.CompleteAsync(), Times.Never);
        }

        #endregion

        #region IsProviderNumberExistsAsync Tests

        [Fact]
        public async Task IsProviderNumberExistsAsync_ReturnsTrue_WhenProviderNumberExists()
        {
            // Arrange
            var providerNumber = "PROV001";

            _mockProviderRepository.Setup(x => x.IsProviderNumberExistsAsync(providerNumber))
                .ReturnsAsync(true);

            // Act
            var result = await _providerService.IsProviderNumberExistsAsync(providerNumber);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsProviderNumberExistsAsync_ReturnsFalse_WhenProviderNumberDoesNotExist()
        {
            // Arrange
            var providerNumber = "NONEXISTENT";

            _mockProviderRepository.Setup(x => x.IsProviderNumberExistsAsync(providerNumber))
                .ReturnsAsync(false);

            // Act
            var result = await _providerService.IsProviderNumberExistsAsync(providerNumber);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region IsNPIExistsAsync Tests

        [Fact]
        public async Task IsNPIExistsAsync_ReturnsTrue_WhenNPIExists()
        {
            // Arrange
            var npi = "1234567890";

            _mockProviderRepository.Setup(x => x.IsNPIExistsAsync(npi))
                .ReturnsAsync(true);

            // Act
            var result = await _providerService.IsNPIExistsAsync(npi);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsNPIExistsAsync_ReturnsFalse_WhenNPIDoesNotExist()
        {
            // Arrange
            var npi = "9999999999";

            _mockProviderRepository.Setup(x => x.IsNPIExistsAsync(npi))
                .ReturnsAsync(false);

            // Act
            var result = await _providerService.IsNPIExistsAsync(npi);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region Helper Methods

        private static Provider CreateTestProvider(int id, string providerNumber, string name, string npi, string specialty)
        {
            return new Provider
            {
                Id = id,
                ProviderNumber = providerNumber,
                Name = name,
                NPI = npi,
                Address = "123 Test Street, Test City, TC 12345",
                Phone = "555-0101",
                Email = $"{name.Replace(" ", ".").Replace("Dr.", "").ToLower()}@test.com",
                Specialty = specialty,
                RxClaims = new List<Domain.Entities.RxClaims.RxClaim>(),
                PriorAuthorizations = new List<Domain.Entities.PriorAuthorization.PriorAuthorizationRecord>()
            };
        }

        #endregion
    }
}