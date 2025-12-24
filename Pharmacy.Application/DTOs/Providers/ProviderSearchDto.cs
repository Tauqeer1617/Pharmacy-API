namespace Pharmacy.Application.DTOs.Providers
{
    public class ProviderSearchCriteriaDto
    {
        public string? ProviderNumber { get; set; }
        public string? Name { get; set; }
        public string? NPI { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Specialty { get; set; }
        public string? Address { get; set; }
        
        // Pagination properties
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        
        // Sorting properties
        public string? SortBy { get; set; } = "Id";
        public bool SortDescending { get; set; } = false;
    }

    public class ProviderSearchResultDto
    {
        public IEnumerable<ProviderDto> Providers { get; set; } = new List<ProviderDto>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}