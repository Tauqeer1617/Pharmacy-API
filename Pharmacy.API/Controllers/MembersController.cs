using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Pharmacy.Application.DTOs.Members;
using Pharmacy.Application.Services.Interfaces;

namespace Pharmacy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly ILogger<MembersController> _logger;

        public MembersController(IMemberService memberService, ILogger<MembersController> logger)
        {
            _memberService = memberService;
            _logger = logger;
        }

        /// <summary>
        /// Get all members
        /// </summary>
        /// <returns>List of members</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetAllMembers()
        {
            try
            {
                var members = await _memberService.GetAllMembersAsync();
                return Ok(members);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all members");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get member by ID
        /// </summary>
        /// <param name="id">Member ID</param>
        /// <returns>Member details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDto>> GetMemberById(int id)
        {
            try
            {
                var member = await _memberService.GetMemberByIdAsync(id);
                if (member == null)
                    return NotFound($"Member with ID {id} not found");

                return Ok(member);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting member by ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get member by member number
        /// </summary>
        /// <param name="memberNumber">Member number</param>
        /// <returns>Member details</returns>
        [HttpGet("by-number/{memberNumber}")]
        public async Task<ActionResult<MemberDto>> GetMemberByMemberNumber(string memberNumber)
        {
            try
            {
                var member = await _memberService.GetMemberByMemberNumberAsync(memberNumber);
                if (member == null)
                    return NotFound($"Member with number {memberNumber} not found");

                return Ok(member);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting member by number {MemberNumber}", memberNumber);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get members by gender
        /// </summary>
        /// <param name="gender">Gender (Male/Female)</param>
        /// <returns>List of members</returns>
        [HttpGet("by-gender/{gender}")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetMembersByGender(string gender)
        {
            try
            {
                var members = await _memberService.GetMembersByGenderAsync(gender);
                return Ok(members);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting members by gender {Gender}", gender);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Search members
        /// </summary>
        /// <param name="searchTerm">Search term (name, member number, email)</param>
        /// <returns>List of matching members</returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> SearchMembers([FromQuery] string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return BadRequest("Search term cannot be empty");

                var members = await _memberService.SearchMembersAsync(searchTerm);
                return Ok(members);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching members with term {SearchTerm}", searchTerm);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Advanced search for members with multiple criteria
        /// </summary>
        /// <param name="searchCriteria">Search criteria including filters, pagination, and sorting</param>
        /// <returns>Paginated search results</returns>
        [HttpPost("advanced-search")]
        public async Task<ActionResult<MemberSearchResultDto>> AdvancedSearchMembers([FromBody] MemberSearchCriteriaDto searchCriteria)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _memberService.AdvancedSearchMembersAsync(searchCriteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while performing advanced search for members");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Create a new member
        /// </summary>
        /// <param name="createMemberDto">Member creation data</param>
        /// <returns>Created member</returns>
        [HttpPost]
        public async Task<ActionResult<MemberDto>> CreateMember([FromBody] CreateMemberDto createMemberDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var member = await _memberService.CreateMemberAsync(createMemberDto);
                return CreatedAtAction(nameof(GetMemberById), new { id = member.Id }, member);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation while creating member");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating member");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Update an existing member
        /// </summary>
        /// <param name="id">Member ID</param>
        /// <param name="updateMemberDto">Member update data</param>
        /// <returns>Updated member</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<MemberDto>> UpdateMember(int id, [FromBody] UpdateMemberDto updateMemberDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var member = await _memberService.UpdateMemberAsync(id, updateMemberDto);
                if (member == null)
                    return NotFound($"Member with ID {id} not found");

                return Ok(member);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating member with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Delete a member
        /// </summary>
        /// <param name="id">Member ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMember(int id)
        {
            try
            {
                var result = await _memberService.DeleteMemberAsync(id);
                if (!result)
                    return NotFound($"Member with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting member with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Check if member number exists
        /// </summary>
        /// <param name="memberNumber">Member number to check</param>
        /// <returns>Boolean indicating existence</returns>
        [HttpGet("exists/{memberNumber}")]
        public async Task<ActionResult<bool>> CheckMemberNumberExists(string memberNumber)
        {
            try
            {
                var exists = await _memberService.IsMemberNumberExistsAsync(memberNumber);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking member number existence {MemberNumber}", memberNumber);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}