using Pharmacy.Application.DTOs.Providers;

namespace Pharmacy.Tests.TestData
{
    public static class ProviderTestDataBuilder
    {
        /// <summary>
        /// Creates a list of sample providers for testing purposes
        /// </summary>
        /// <returns>List of CreateProviderDto objects for testing</returns>
        public static List<CreateProviderDto> GetSampleProviders()
        {
            return new List<CreateProviderDto>
            {
                new CreateProviderDto
                {
                    ProviderNumber = "PROV001",
                    Name = "Dr. Sarah Johnson",
                    NPI = "1234567890",
                    Address = "123 Medical Center Dr, New York, NY 10001",
                    Phone = "555-0201",
                    Email = "sarah.johnson@medcenter.com",
                    Specialty = "Family Medicine"
                },
                new CreateProviderDto
                {
                    ProviderNumber = "PROV002",
                    Name = "Dr. Michael Chen",
                    NPI = "2345678901",
                    Address = "456 Cardiology Ave, Los Angeles, CA 90210",
                    Phone = "555-0202",
                    Email = "michael.chen@cardio.com",
                    Specialty = "Cardiology"
                },
                new CreateProviderDto
                {
                    ProviderNumber = "PROV003",
                    Name = "Dr. Emily Rodriguez",
                    NPI = "3456789012",
                    Address = "789 Pediatric Way, Chicago, IL 60601",
                    Phone = "555-0203",
                    Email = "emily.rodriguez@pediatrics.com",
                    Specialty = "Pediatrics"
                },
                new CreateProviderDto
                {
                    ProviderNumber = "PROV004",
                    Name = "Dr. David Kumar",
                    NPI = "4567890123",
                    Address = "321 Orthopedic Blvd, Houston, TX 77001",
                    Phone = "555-0204",
                    Email = "david.kumar@orthopedics.com",
                    Specialty = "Orthopedics"
                },
                new CreateProviderDto
                {
                    ProviderNumber = "PROV005",
                    Name = "Dr. Lisa Thompson",
                    NPI = "5678901234",
                    Address = "654 Dermatology St, Phoenix, AZ 85001",
                    Phone = "555-0205",
                    Email = "lisa.thompson@dermatology.com",
                    Specialty = "Dermatology"
                },
                new CreateProviderDto
                {
                    ProviderNumber = "PROV006",
                    Name = "Dr. Robert Williams",
                    NPI = "6789012345",
                    Address = "987 Neurology Rd, Philadelphia, PA 19101",
                    Phone = "555-0206",
                    Email = "robert.williams@neuro.com",
                    Specialty = "Neurology"
                }
            };
        }

        /// <summary>
        /// Creates a single sample provider for testing
        /// </summary>
        /// <param name="providerNumber">Optional provider number (defaults to TEST001)</param>
        /// <param name="npi">Optional NPI (defaults to 9999999999)</param>
        /// <returns>CreateProviderDto for testing</returns>
        public static CreateProviderDto GetSingleTestProvider(string providerNumber = "TEST001", string npi = "9999999999")
        {
            return new CreateProviderDto
            {
                ProviderNumber = providerNumber,
                Name = "Dr. Test Provider",
                NPI = npi,
                Address = "123 Test Street, Test City, TC 12345",
                Phone = "555-TEST",
                Email = "test.provider@test.com",
                Specialty = "Test Medicine"
            };
        }

        /// <summary>
        /// Creates an invalid provider for testing validation
        /// </summary>
        /// <returns>CreateProviderDto with invalid data</returns>
        public static CreateProviderDto GetInvalidProvider()
        {
            return new CreateProviderDto
            {
                ProviderNumber = "", // Invalid - empty
                Name = "", // Invalid - empty
                NPI = "123", // Invalid - not 10 digits
                Address = "", // Invalid - empty
                Phone = "", // Invalid - empty
                Email = "invalid-email", // Invalid - not proper email format
                Specialty = "" // Invalid - empty
            };
        }

        /// <summary>
        /// Creates valid update data for testing provider updates
        /// </summary>
        /// <returns>UpdateProviderDto for testing</returns>
        public static UpdateProviderDto GetValidUpdateData()
        {
            return new UpdateProviderDto
            {
                Name = "Dr. Updated Provider",
                NPI = "1111111111",
                Address = "456 Updated Street, Updated City, UC 54321",
                Phone = "555-UPDT",
                Email = "updated.provider@test.com",
                Specialty = "Updated Medicine"
            };
        }

        /// <summary>
        /// Creates search criteria for testing advanced search functionality
        /// </summary>
        /// <returns>ProviderSearchCriteriaDto for testing</returns>
        public static ProviderSearchCriteriaDto GetTestSearchCriteria()
        {
            return new ProviderSearchCriteriaDto
            {
                Name = "Dr.",
                Specialty = "Medicine",
                PageNumber = 1,
                PageSize = 10,
                SortBy = "Name",
                SortDescending = false
            };
        }
    }
}