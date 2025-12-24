namespace Pharmacy.Application.DTOs.Members
{
    public class MemberSearchCriteriaDto
    {
        public string? MemberNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirthFrom { get; set; }
        public DateTime? DateOfBirthTo { get; set; }
        public int? AgeFrom { get; set; }
        public int? AgeTo { get; set; }
        
        // Pagination properties
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        
        // Sorting properties
        public string? SortBy { get; set; } = "Id";
        public bool SortDescending { get; set; } = false;
    }

    public class MemberSearchResultDto
    {
        public IEnumerable<MemberDto> Members { get; set; } = new List<MemberDto>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}