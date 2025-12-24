namespace Pharmacy.Application.DTOs.Providers
{
    public class ProviderDto
    {
        public int Id { get; set; }
        public string ProviderNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string NPI { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
    }

    public class CreateProviderDto
    {
        public string ProviderNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string NPI { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
    }

    public class UpdateProviderDto
    {
        public string Name { get; set; } = string.Empty;
        public string NPI { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
    }
}