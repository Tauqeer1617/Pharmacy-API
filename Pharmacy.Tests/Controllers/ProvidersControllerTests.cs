using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Pharmacy.API.Controllers;
using Pharmacy.Application.DTOs.Providers;
using Pharmacy.Application.Services.Interfaces;
using Pharmacy.Tests.TestData;

namespace Pharmacy.Tests.Controllers
{
    public class ProvidersControllerTests
    {
        private readonly Mock<IProviderService> _mockProviderService;
        private readonly Mock<ILogger<ProvidersController>> _mockLogger;
        private readonly ProvidersController _controller;

        public ProvidersControllerTests()
        {
            _mockProviderService = new Mock<IProviderService>();
            _mockLogger = new Mock<ILogger<ProvidersController>>();
            _controller = new ProvidersController(_mockProviderService.Object, _mockLogger.Object);
        }

        #region GetAllProviders Tests

        [Fact]
        public async Task GetAllProviders_ReturnsOkResult_WithListOfProviders()
        {
            // Arrange
            var expectedProviders = new List<ProviderDto>
            {
                new ProviderDto { Id = 1, Name = "Dr. John Doe", ProviderNumber = "PROV001", NPI = "1234567890", Specialty = "Family Medicine", Email = "john@test.com", Phone = "555-0101", Address = "123 Main St" },
                new ProviderDto { Id = 2, Name = "Dr. Jane Smith", ProviderNumber = "PROV002", NPI = "2345678901", Specialty = "Cardiology", Email = "jane@test.com", Phone = "555-0102", Address = "456 Oak Ave" }
            };

            _mockProviderService.Setup(x => x.GetAllProvidersAsync())
                .ReturnsAsync(expectedProviders);

            // Act
            var result = await _controller.GetAllProviders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualProviders = Assert.IsAssignableFrom<IEnumerable<ProviderDto>>(okResult.Value);
            Assert.Equal(2, actualProviders.Count());
            Assert.Equal(expectedProviders, actualProviders);
        }

        [Fact]
        public async Task GetAllProviders_ReturnsInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            _mockProviderService.Setup(x => x.GetAllProvidersAsync())
                .ThrowsAsync(new Exception("Database connection failed"));

            // Act
            var result = await _controller.GetAllProviders();

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusResult.StatusCode);
            Assert.Equal("Internal server error", statusResult.Value);
        }

        #endregion

        #region GetProviderById Tests

        [Fact]
        public async Task GetProviderById_ReturnsOkResult_WhenProviderExists()
        {
            // Arrange
            var providerId = 1;
            var expectedProvider = new ProviderDto 
            { 
                Id = providerId, 
                Name = "Dr. John Doe", 
                ProviderNumber = "PROV001",
                NPI = "1234567890",
                Specialty = "Family Medicine",
                Email = "john@test.com",
                Phone = "555-0101",
                Address = "123 Main St"
            };

            _mockProviderService.Setup(x => x.GetProviderByIdAsync(providerId))
                .ReturnsAsync(expectedProvider);

            // Act
            var result = await _controller.GetProviderById(providerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualProvider = Assert.IsType<ProviderDto>(okResult.Value);
            Assert.Equal(expectedProvider.Id, actualProvider.Id);
            Assert.Equal(expectedProvider.Name, actualProvider.Name);
        }

        [Fact]
        public async Task GetProviderById_ReturnsNotFound_WhenProviderDoesNotExist()
        {
            // Arrange
            var providerId = 999;
            _mockProviderService.Setup(x => x.GetProviderByIdAsync(providerId))
                .ReturnsAsync((ProviderDto?)null);

            // Act
            var result = await _controller.GetProviderById(providerId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"Provider with ID {providerId} not found", notFoundResult.Value);
        }

        [Fact]
        public async Task GetProviderById_ReturnsInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            var providerId = 1;
            _mockProviderService.Setup(x => x.GetProviderByIdAsync(providerId))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetProviderById(providerId);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        #endregion

        #region GetProviderByProviderNumber Tests

        [Fact]
        public async Task GetProviderByProviderNumber_ReturnsOkResult_WhenProviderExists()
        {
            // Arrange
            var providerNumber = "PROV001";
            var expectedProvider = new ProviderDto 
            { 
                Id = 1, 
                Name = "Dr. John Doe", 
                ProviderNumber = providerNumber,
                NPI = "1234567890",
                Specialty = "Family Medicine",
                Email = "john@test.com",
                Phone = "555-0101",
                Address = "123 Main St"
            };

            _mockProviderService.Setup(x => x.GetProviderByProviderNumberAsync(providerNumber))
                .ReturnsAsync(expectedProvider);

            // Act
            var result = await _controller.GetProviderByProviderNumber(providerNumber);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualProvider = Assert.IsType<ProviderDto>(okResult.Value);
            Assert.Equal(expectedProvider.ProviderNumber, actualProvider.ProviderNumber);
        }

        [Fact]
        public async Task GetProviderByProviderNumber_ReturnsNotFound_WhenProviderDoesNotExist()
        {
            // Arrange
            var providerNumber = "NONEXISTENT";
            _mockProviderService.Setup(x => x.GetProviderByProviderNumberAsync(providerNumber))
                .ReturnsAsync((ProviderDto?)null);

            // Act
            var result = await _controller.GetProviderByProviderNumber(providerNumber);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"Provider with number {providerNumber} not found", notFoundResult.Value);
        }

        #endregion

        #region GetProviderByNPI Tests

        [Fact]
        public async Task GetProviderByNPI_ReturnsOkResult_WhenProviderExists()
        {
            // Arrange
            var npi = "1234567890";
            var expectedProvider = new ProviderDto 
            { 
                Id = 1, 
                Name = "Dr. John Doe", 
                ProviderNumber = "PROV001",
                NPI = npi,
                Specialty = "Family Medicine",
                Email = "john@test.com",
                Phone = "555-0101",
                Address = "123 Main St"
            };

            _mockProviderService.Setup(x => x.GetProviderByNPIAsync(npi))
                .ReturnsAsync(expectedProvider);

            // Act
            var result = await _controller.GetProviderByNPI(npi);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualProvider = Assert.IsType<ProviderDto>(okResult.Value);
            Assert.Equal(expectedProvider.NPI, actualProvider.NPI);
        }

        [Fact]
        public async Task GetProviderByNPI_ReturnsNotFound_WhenProviderDoesNotExist()
        {
            // Arrange
            var npi = "9999999999";
            _mockProviderService.Setup(x => x.GetProviderByNPIAsync(npi))
                .ReturnsAsync((ProviderDto?)null);

            // Act
            var result = await _controller.GetProviderByNPI(npi);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"Provider with NPI {npi} not found", notFoundResult.Value);
        }

        #endregion

        #region GetProvidersBySpecialty Tests

        [Fact]
        public async Task GetProvidersBySpecialty_ReturnsOkResult_WithFilteredProviders()
        {
            // Arrange
            var specialty = "Cardiology";
            var expectedProviders = new List<ProviderDto>
            {
                new ProviderDto { Id = 1, Name = "Dr. Heart Specialist", Specialty = specialty, ProviderNumber = "CARD001", NPI = "1111111111", Email = "heart@test.com", Phone = "555-0201", Address = "123 Heart St" }
            };

            _mockProviderService.Setup(x => x.GetProvidersBySpecialtyAsync(specialty))
                .ReturnsAsync(expectedProviders);

            // Act
            var result = await _controller.GetProvidersBySpecialty(specialty);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualProviders = Assert.IsAssignableFrom<IEnumerable<ProviderDto>>(okResult.Value);
            Assert.Single(actualProviders);
            Assert.All(actualProviders, p => Assert.Equal(specialty, p.Specialty));
        }

        #endregion

        #region SearchProviders Tests

        [Fact]
        public async Task SearchProviders_ReturnsOkResult_WithMatchingProviders()
        {
            // Arrange
            var searchTerm = "Kumar";
            var expectedProviders = new List<ProviderDto>
            {
                new ProviderDto { Id = 1, Name = "Dr. David Kumar", ProviderNumber = "PROV004", NPI = "4567890123", Specialty = "Orthopedics", Email = "kumar@test.com", Phone = "555-0204", Address = "321 Kumar Blvd" }
            };

            _mockProviderService.Setup(x => x.SearchProvidersAsync(searchTerm))
                .ReturnsAsync(expectedProviders);

            // Act
            var result = await _controller.SearchProviders(searchTerm);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualProviders = Assert.IsAssignableFrom<IEnumerable<ProviderDto>>(okResult.Value);
            Assert.Single(actualProviders);
            Assert.Contains("Kumar", actualProviders.First().Name);
        }

        [Fact]
        public async Task SearchProviders_ReturnsBadRequest_WhenSearchTermIsEmpty()
        {
            // Arrange
            var searchTerm = "";

            // Act
            var result = await _controller.SearchProviders(searchTerm);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Search term cannot be empty", badRequestResult.Value);
        }

        [Fact]
        public async Task SearchProviders_ReturnsBadRequest_WhenSearchTermIsWhitespace()
        {
            // Arrange
            var searchTerm = "   ";

            // Act
            var result = await _controller.SearchProviders(searchTerm);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Search term cannot be empty", badRequestResult.Value);
        }

        #endregion

        #region AdvancedSearchProviders Tests

        [Fact]
        public async Task AdvancedSearchProviders_ReturnsOkResult_WithSearchResults()
        {
            // Arrange
            var searchCriteria = ProviderTestDataBuilder.GetTestSearchCriteria();
            var expectedResult = new ProviderSearchResultDto
            {
                Providers = new List<ProviderDto>
                {
                    new ProviderDto { Id = 1, Name = "Dr. Test", ProviderNumber = "TEST001", NPI = "1111111111", Specialty = "Test Medicine", Email = "test@test.com", Phone = "555-TEST", Address = "123 Test St" }
                },
                TotalCount = 1,
                PageNumber = 1,
                PageSize = 10,
                TotalPages = 1,
                HasPreviousPage = false,
                HasNextPage = false
            };

            _mockProviderService.Setup(x => x.AdvancedSearchProvidersAsync(It.IsAny<ProviderSearchCriteriaDto>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.AdvancedSearchProviders(searchCriteria);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualResult = Assert.IsType<ProviderSearchResultDto>(okResult.Value);
            Assert.Equal(expectedResult.TotalCount, actualResult.TotalCount);
            Assert.Equal(expectedResult.PageNumber, actualResult.PageNumber);
        }

        [Fact]
        public async Task AdvancedSearchProviders_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var searchCriteria = new ProviderSearchCriteriaDto
            {
                PageNumber = 0, // Invalid - must be > 0
                PageSize = 0    // Invalid - must be > 0
            };

            _controller.ModelState.AddModelError("PageNumber", "Page number must be greater than 0");

            // Act
            var result = await _controller.AdvancedSearchProviders(searchCriteria);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        #endregion

        #region CreateProvider Tests

        [Fact]
        public async Task CreateProvider_ReturnsCreatedAtActionResult_WhenProviderIsCreated()
        {
            // Arrange
            var createDto = ProviderTestDataBuilder.GetSingleTestProvider();
            var createdProvider = new ProviderDto
            {
                Id = 1,
                ProviderNumber = createDto.ProviderNumber,
                Name = createDto.Name,
                NPI = createDto.NPI,
                Address = createDto.Address,
                Phone = createDto.Phone,
                Email = createDto.Email,
                Specialty = createDto.Specialty
            };

            _mockProviderService.Setup(x => x.CreateProviderAsync(It.IsAny<CreateProviderDto>()))
                .ReturnsAsync(createdProvider);

            // Act
            var result = await _controller.CreateProvider(createDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(ProvidersController.GetProviderById), createdResult.ActionName);
            Assert.NotNull(createdResult.RouteValues);
            Assert.True(createdResult.RouteValues.ContainsKey("id"));
            Assert.Equal(createdProvider.Id, createdResult.RouteValues["id"]);
            var actualProvider = Assert.IsType<ProviderDto>(createdResult.Value);
            Assert.Equal(createdProvider.Id, actualProvider.Id);
        }

        [Fact]
        public async Task CreateProvider_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var createDto = new CreateProviderDto(); // Invalid - missing required fields
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.CreateProvider(createDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public async Task CreateProvider_ReturnsBadRequest_WhenInvalidOperationExceptionThrown()
        {
            // Arrange
            var createDto = ProviderTestDataBuilder.GetSingleTestProvider();
            var exceptionMessage = "Provider number already exists";

            _mockProviderService.Setup(x => x.CreateProviderAsync(It.IsAny<CreateProviderDto>()))
                .ThrowsAsync(new InvalidOperationException(exceptionMessage));

            // Act
            var result = await _controller.CreateProvider(createDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(exceptionMessage, badRequestResult.Value);
        }

        [Fact]
        public async Task CreateProvider_ReturnsInternalServerError_WhenUnexpectedExceptionThrown()
        {
            // Arrange
            var createDto = ProviderTestDataBuilder.GetSingleTestProvider();

            _mockProviderService.Setup(x => x.CreateProviderAsync(It.IsAny<CreateProviderDto>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.CreateProvider(createDto);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusResult.StatusCode);
            Assert.Equal("Internal server error", statusResult.Value);
        }

        #endregion

        #region UpdateProvider Tests

        [Fact]
        public async Task UpdateProvider_ReturnsOkResult_WhenProviderIsUpdated()
        {
            // Arrange
            var providerId = 1;
            var updateDto = ProviderTestDataBuilder.GetValidUpdateData();
            var updatedProvider = new ProviderDto
            {
                Id = providerId,
                ProviderNumber = "PROV001",
                Name = updateDto.Name,
                NPI = updateDto.NPI,
                Address = updateDto.Address,
                Phone = updateDto.Phone,
                Email = updateDto.Email,
                Specialty = updateDto.Specialty
            };

            _mockProviderService.Setup(x => x.UpdateProviderAsync(providerId, It.IsAny<UpdateProviderDto>()))
                .ReturnsAsync(updatedProvider);

            // Act
            var result = await _controller.UpdateProvider(providerId, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualProvider = Assert.IsType<ProviderDto>(okResult.Value);
            Assert.Equal(updatedProvider.Id, actualProvider.Id);
            Assert.Equal(updateDto.Name, actualProvider.Name);
        }

        [Fact]
        public async Task UpdateProvider_ReturnsNotFound_WhenProviderDoesNotExist()
        {
            // Arrange
            var providerId = 999;
            var updateDto = ProviderTestDataBuilder.GetValidUpdateData();

            _mockProviderService.Setup(x => x.UpdateProviderAsync(providerId, It.IsAny<UpdateProviderDto>()))
                .ReturnsAsync((ProviderDto?)null);

            // Act
            var result = await _controller.UpdateProvider(providerId, updateDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"Provider with ID {providerId} not found", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateProvider_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var providerId = 1;
            var updateDto = new UpdateProviderDto(); // Invalid
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.UpdateProvider(providerId, updateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        #endregion

        #region DeleteProvider Tests

        [Fact]
        public async Task DeleteProvider_ReturnsNoContent_WhenProviderIsDeleted()
        {
            // Arrange
            var providerId = 1;

            _mockProviderService.Setup(x => x.DeleteProviderAsync(providerId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteProvider(providerId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProvider_ReturnsNotFound_WhenProviderDoesNotExist()
        {
            // Arrange
            var providerId = 999;

            _mockProviderService.Setup(x => x.DeleteProviderAsync(providerId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteProvider(providerId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Provider with ID {providerId} not found", notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteProvider_ReturnsInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            var providerId = 1;

            _mockProviderService.Setup(x => x.DeleteProviderAsync(providerId))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.DeleteProvider(providerId);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        #endregion

        #region CheckProviderNumberExists Tests

        [Fact]
        public async Task CheckProviderNumberExists_ReturnsTrue_WhenProviderNumberExists()
        {
            // Arrange
            var providerNumber = "PROV001";

            _mockProviderService.Setup(x => x.IsProviderNumberExistsAsync(providerNumber))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.CheckProviderNumberExists(providerNumber);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)okResult.Value!);
        }

        [Fact]
        public async Task CheckProviderNumberExists_ReturnsFalse_WhenProviderNumberDoesNotExist()
        {
            // Arrange
            var providerNumber = "NONEXISTENT";

            _mockProviderService.Setup(x => x.IsProviderNumberExistsAsync(providerNumber))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.CheckProviderNumberExists(providerNumber);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.False((bool)okResult.Value!);
        }

        #endregion

        #region CheckNPIExists Tests

        [Fact]
        public async Task CheckNPIExists_ReturnsTrue_WhenNPIExists()
        {
            // Arrange
            var npi = "1234567890";

            _mockProviderService.Setup(x => x.IsNPIExistsAsync(npi))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.CheckNPIExists(npi);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)okResult.Value!);
        }

        [Fact]
        public async Task CheckNPIExists_ReturnsFalse_WhenNPIDoesNotExist()
        {
            // Arrange
            var npi = "9999999999";

            _mockProviderService.Setup(x => x.IsNPIExistsAsync(npi))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.CheckNPIExists(npi);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.False((bool)okResult.Value!);
        }

        #endregion
    }
}