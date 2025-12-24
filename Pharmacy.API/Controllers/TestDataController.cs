using Microsoft.AspNetCore.Mvc;
using Pharmacy.Application.DTOs.Members;
using Pharmacy.Application.Services.Interfaces;

namespace Pharmacy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestDataController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly ILogger<TestDataController> _logger;

        public TestDataController(IMemberService memberService, ILogger<TestDataController> logger)
        {
            _memberService = memberService;
            _logger = logger;
        }

        /// <summary>
        /// Create test members for demonstration
        /// </summary>
        /// <returns>Created test members</returns>
        [HttpPost("create-test-members")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> CreateTestMembers()
        {
            try
            {
                var testMembers = new List<CreateMemberDto>
                {
                    new CreateMemberDto
                    {
                        MemberNumber = "MEM001",
                        FirstName = "John",
                        LastName = "Doe",
                        DOB = new DateTime(1985, 5, 15),
                        Gender = "Male",
                        Address = "123 Main Street, New York, NY 10001",
                        Phone = "555-0101",
                        Email = "john.doe@email.com"
                    },
                    new CreateMemberDto
                    {
                        MemberNumber = "MEM002",
                        FirstName = "Jane",
                        LastName = "Smith",
                        DOB = new DateTime(1990, 8, 22),
                        Gender = "Female",
                        Address = "456 Oak Avenue, Los Angeles, CA 90210",
                        Phone = "555-0102",
                        Email = "jane.smith@email.com"
                    },
                    new CreateMemberDto
                    {
                        MemberNumber = "MEM003",
                        FirstName = "Michael",
                        LastName = "Johnson",
                        DOB = new DateTime(1982, 12, 10),
                        Gender = "Male",
                        Address = "789 Pine Street, Chicago, IL 60601",
                        Phone = "555-0103",
                        Email = "michael.johnson@email.com"
                    },
                    new CreateMemberDto
                    {
                        MemberNumber = "MEM004",
                        FirstName = "Emily",
                        LastName = "Davis",
                        DOB = new DateTime(1995, 3, 8),
                        Gender = "Female",
                        Address = "321 Elm Drive, Houston, TX 77001",
                        Phone = "555-0104",
                        Email = "emily.davis@email.com"
                    },
                    new CreateMemberDto
                    {
                        MemberNumber = "MEM005",
                        FirstName = "David",
                        LastName = "Wilson",
                        DOB = new DateTime(1988, 7, 25),
                        Gender = "Male",
                        Address = "654 Maple Road, Phoenix, AZ 85001",
                        Phone = "555-0105",
                        Email = "david.wilson@email.com"
                    },
                    new CreateMemberDto
                    {
                        MemberNumber = "KUMAR001",
                        FirstName = "Rajesh",
                        LastName = "Kumar",
                        DOB = new DateTime(1985, 3, 15),
                        Gender = "Male",
                        Address = "123 Kumar Street, Delhi, India",
                        Phone = "555-KUMAR",
                        Email = "rajesh.kumar@email.com"
                    }
                };

                var createdMembers = new List<MemberDto>();
                
                foreach (var memberDto in testMembers)
                {
                    try
                    {
                        // Check if member already exists
                        var existingMember = await _memberService.GetMemberByMemberNumberAsync(memberDto.MemberNumber);
                        if (existingMember == null)
                        {
                            var createdMember = await _memberService.CreateMemberAsync(memberDto);
                            createdMembers.Add(createdMember);
                            _logger.LogInformation("Created test member: {MemberNumber}", memberDto.MemberNumber);
                        }
                        else
                        {
                            createdMembers.Add(existingMember);
                            _logger.LogInformation("Test member already exists: {MemberNumber}", memberDto.MemberNumber);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to create test member: {MemberNumber}", memberDto.MemberNumber);
                    }
                }

                return Ok(new { 
                    Message = $"Test data creation completed. {createdMembers.Count} members available.",
                    Members = createdMembers,
                    TotalCount = createdMembers.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating test members");
                return StatusCode(500, "Internal server error while creating test data");
            }
        }

        /// <summary>
        /// Get count of existing members
        /// </summary>
        /// <returns>Member count</returns>
        [HttpGet("member-count")]
        public async Task<ActionResult<object>> GetMemberCount()
        {
            try
            {
                var members = await _memberService.GetAllMembersAsync();
                var count = members.Count();
                
                return Ok(new { 
                    TotalMembers = count,
                    Message = count > 0 ? $"Found {count} members in database" : "No members found - consider creating test data"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting member count");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get all members with their last names for debugging
        /// </summary>
        /// <returns>List of all members with focus on last names</returns>
        [HttpGet("debug-members")]
        public async Task<ActionResult<object>> DebugMembers()
        {
            try
            {
                var allMembers = await _memberService.GetAllMembersAsync();
                var memberInfo = allMembers.Select(m => new
                {
                    m.Id,
                    m.MemberNumber,
                    m.FirstName,
                    m.LastName,
                    LastNameLower = m.LastName?.ToLower(),
                    m.Email,
                    m.Gender
                }).ToList();

                return Ok(new
                {
                    TotalMembers = memberInfo.Count,
                    Members = memberInfo,
                    LastNames = memberInfo.Select(m => m.LastName).Distinct().ToList(),
                    ContainsKumar = memberInfo.Any(m => 
                        m.LastName != null && 
                        m.LastName.Contains("kumar", StringComparison.OrdinalIgnoreCase))
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while debugging members");
                return StatusCode(500, "Internal server error while debugging");
            }
        }

        /// <summary>
        /// Test search with specific criteria for debugging
        /// </summary>
        /// <param name="lastName">Last name to search for</param>
        /// <returns>Search results</returns>
        [HttpGet("test-search")]
        public async Task<ActionResult<object>> TestSearch([FromQuery] string? lastName = null)
        {
            try
            {
                if (string.IsNullOrEmpty(lastName))
                {
                    lastName = "kumar"; // Default test value
                }

                var searchCriteria = new MemberSearchCriteriaDto
                {
                    LastName = lastName,
                    PageNumber = 1,
                    PageSize = 50
                };

                _logger.LogInformation("Testing search with LastName: {LastName}", lastName);

                var result = await _memberService.AdvancedSearchMembersAsync(searchCriteria);

                return Ok(new
                {
                    SearchCriteria = searchCriteria,
                    ResultCount = result.TotalCount,
                    Members = result.Members.Select(m => new
                    {
                        m.Id,
                        m.MemberNumber,
                        m.FirstName,
                        m.LastName,
                        m.Email
                    }),
                    Message = result.TotalCount > 0 
                        ? $"Found {result.TotalCount} members with last name containing '{lastName}'" 
                        : $"No members found with last name containing '{lastName}'"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during test search with LastName: {LastName}", lastName);
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Clear all test members (use with caution)
        /// </summary>
        /// <returns>Deletion result</returns>
        [HttpDelete("clear-test-members")]
        public async Task<ActionResult<object>> ClearTestMembers()
        {
            try
            {
                var allMembers = await _memberService.GetAllMembersAsync();
                var deletedCount = 0;

                foreach (var member in allMembers.Where(m => m.MemberNumber.StartsWith("MEM") || m.MemberNumber.StartsWith("KUMAR")))
                {
                    var deleted = await _memberService.DeleteMemberAsync(member.Id);
                    if (deleted)
                    {
                        deletedCount++;
                        _logger.LogInformation("Deleted test member: {MemberNumber}", member.MemberNumber);
                    }
                }

                return Ok(new { 
                    Message = $"Deleted {deletedCount} test members",
                    DeletedCount = deletedCount
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while clearing test members");
                return StatusCode(500, "Internal server error while clearing test data");
            }
        }
    }
}