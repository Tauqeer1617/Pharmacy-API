using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Pharmacy.Application.DTOs.Providers;
using Pharmacy.Application.Services.Interfaces;

namespace Pharmacy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors]
    public class ProvidersController : ControllerBase
    {
        private readonly IProviderService _providerService;
        private readonly ILogger<ProvidersController> _logger;

        public ProvidersController(IProviderService providerService, ILogger<ProvidersController> logger)
        {
            _providerService = providerService;
            _logger = logger;
        }

        /// <summary>
        /// Get all providers
        /// </summary>
        /// <returns>List of providers</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProviderDto>>> GetAllProviders()
        {
            try
            {
                var providers = await _providerService.GetAllProvidersAsync();
                return Ok(providers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all providers");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get provider by ID
        /// </summary>
        /// <param name="id">Provider ID</param>
        /// <returns>Provider details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProviderDto>> GetProviderById(int id)
        {
            try
            {
                var provider = await _providerService.GetProviderByIdAsync(id);
                if (provider == null)
                    return NotFound($"Provider with ID {id} not found");

                return Ok(provider);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting provider by ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get provider by provider number
        /// </summary>
        /// <param name="providerNumber">Provider number</param>
        /// <returns>Provider details</returns>
        [HttpGet("by-number/{providerNumber}")]
        public async Task<ActionResult<ProviderDto>> GetProviderByProviderNumber(string providerNumber)
        {
            try
            {
                var provider = await _providerService.GetProviderByProviderNumberAsync(providerNumber);
                if (provider == null)
                    return NotFound($"Provider with number {providerNumber} not found");

                return Ok(provider);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting provider by number {ProviderNumber}", providerNumber);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get provider by NPI
        /// </summary>
        /// <param name="npi">National Provider Identifier</param>
        /// <returns>Provider details</returns>
        [HttpGet("by-npi/{npi}")]
        public async Task<ActionResult<ProviderDto>> GetProviderByNPI(string npi)
        {
            try
            {
                var provider = await _providerService.GetProviderByNPIAsync(npi);
                if (provider == null)
                    return NotFound($"Provider with NPI {npi} not found");

                return Ok(provider);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting provider by NPI {NPI}", npi);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get providers by specialty
        /// </summary>
        /// <param name="specialty">Provider specialty</param>
        /// <returns>List of providers</returns>
        [HttpGet("by-specialty/{specialty}")]
        public async Task<ActionResult<IEnumerable<ProviderDto>>> GetProvidersBySpecialty(string specialty)
        {
            try
            {
                var providers = await _providerService.GetProvidersBySpecialtyAsync(specialty);
                return Ok(providers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting providers by specialty {Specialty}", specialty);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Search providers
        /// </summary>
        /// <param name="searchTerm">Search term (name, provider number, NPI, email, specialty)</param>
        /// <returns>List of matching providers</returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProviderDto>>> SearchProviders([FromQuery] string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return BadRequest("Search term cannot be empty");

                var providers = await _providerService.SearchProvidersAsync(searchTerm);
                return Ok(providers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching providers with term {SearchTerm}", searchTerm);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Advanced search for providers with multiple criteria
        /// </summary>
        /// <param name="searchCriteria">Search criteria including filters, pagination, and sorting</param>
        /// <returns>Paginated search results</returns>
        [HttpPost("advanced-search")]
        public async Task<ActionResult<ProviderSearchResultDto>> AdvancedSearchProviders([FromBody] ProviderSearchCriteriaDto searchCriteria)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _providerService.AdvancedSearchProvidersAsync(searchCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while performing advanced search for providers");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Create a new provider
        /// </summary>
        /// <param name="createProviderDto">Provider creation data</param>
        /// <returns>Created provider</returns>
        [HttpPost]
        public async Task<ActionResult<ProviderDto>> CreateProvider([FromBody] CreateProviderDto createProviderDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var provider = await _providerService.CreateProviderAsync(createProviderDto);
                return CreatedAtAction(nameof(GetProviderById), new { id = provider.Id }, provider);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation while creating provider");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating provider");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Update an existing provider
        /// </summary>
        /// <param name="id">Provider ID</param>
        /// <param name="updateProviderDto">Provider update data</param>
        /// <returns>Updated provider</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProviderDto>> UpdateProvider(int id, [FromBody] UpdateProviderDto updateProviderDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var provider = await _providerService.UpdateProviderAsync(id, updateProviderDto);
                if (provider == null)
                    return NotFound($"Provider with ID {id} not found");

                return Ok(provider);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation while updating provider");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating provider with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Delete a provider
        /// </summary>
        /// <param name="id">Provider ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProvider(int id)
        {
            try
            {
                var result = await _providerService.DeleteProviderAsync(id);
                if (!result)
                    return NotFound($"Provider with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting provider with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Check if provider number exists
        /// </summary>
        /// <param name="providerNumber">Provider number to check</param>
        /// <returns>Boolean indicating existence</returns>
        [HttpGet("exists/provider-number/{providerNumber}")]
        public async Task<ActionResult<bool>> CheckProviderNumberExists(string providerNumber)
        {
            try
            {
                var exists = await _providerService.IsProviderNumberExistsAsync(providerNumber);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking provider number existence {ProviderNumber}", providerNumber);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Check if NPI exists
        /// </summary>
        /// <param name="npi">NPI to check</param>
        /// <returns>Boolean indicating existence</returns>
        [HttpGet("exists/npi/{npi}")]
        public async Task<ActionResult<bool>> CheckNPIExists(string npi)
        {
            try
            {
                var exists = await _providerService.IsNPIExistsAsync(npi);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking NPI existence {NPI}", npi);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}