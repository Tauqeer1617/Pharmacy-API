using Pharmacy.Tests.TestData;

namespace Pharmacy.Tests.Unit
{
    public class ProviderTestDataBuilderTests
    {
        [Fact]
        public void GetSampleProviders_Should_Return_Six_Providers()
        {
            // Act
            var providers = ProviderTestDataBuilder.GetSampleProviders();

            // Assert
            Assert.NotNull(providers);
            Assert.Equal(6, providers.Count);
            
            // Verify we have the expected test providers
            Assert.Contains(providers, p => p.ProviderNumber == "PROV001");
            Assert.Contains(providers, p => p.ProviderNumber == "PROV002");
            Assert.Contains(providers, p => p.Name == "Dr. David Kumar");
            Assert.Contains(providers, p => p.Specialty == "Cardiology");
        }

        [Fact]
        public void GetSingleTestProvider_Should_Return_Valid_Provider()
        {
            // Act
            var provider = ProviderTestDataBuilder.GetSingleTestProvider();

            // Assert
            Assert.NotNull(provider);
            Assert.Equal("TEST001", provider.ProviderNumber);
            Assert.Equal("Dr. Test Provider", provider.Name);
            Assert.Equal("9999999999", provider.NPI);
            Assert.Equal("Test Medicine", provider.Specialty);
            Assert.Contains("@", provider.Email);
        }

        [Fact]
        public void GetSingleTestProvider_WithCustomValues_Should_Use_Provided_Values()
        {
            // Arrange
            var customProviderNumber = "CUSTOM123";
            var customNPI = "1111111111";

            // Act
            var provider = ProviderTestDataBuilder.GetSingleTestProvider(customProviderNumber, customNPI);

            // Assert
            Assert.Equal(customProviderNumber, provider.ProviderNumber);
            Assert.Equal(customNPI, provider.NPI);
        }

        [Fact]
        public void GetInvalidProvider_Should_Return_Provider_With_Invalid_Data()
        {
            // Act
            var provider = ProviderTestDataBuilder.GetInvalidProvider();

            // Assert
            Assert.NotNull(provider);
            Assert.Equal(string.Empty, provider.ProviderNumber);
            Assert.Equal(string.Empty, provider.Name);
            Assert.Equal("123", provider.NPI); // Invalid - not 10 digits
            Assert.Equal("invalid-email", provider.Email); // Invalid format
        }

        [Fact]
        public void GetValidUpdateData_Should_Return_Valid_Update_DTO()
        {
            // Act
            var updateData = ProviderTestDataBuilder.GetValidUpdateData();

            // Assert
            Assert.NotNull(updateData);
            Assert.Equal("Dr. Updated Provider", updateData.Name);
            Assert.Equal("1111111111", updateData.NPI);
            Assert.Contains("@", updateData.Email);
            Assert.Equal("Updated Medicine", updateData.Specialty);
        }

        [Fact]
        public void GetTestSearchCriteria_Should_Return_Valid_Search_Criteria()
        {
            // Act
            var criteria = ProviderTestDataBuilder.GetTestSearchCriteria();

            // Assert
            Assert.NotNull(criteria);
            Assert.Equal("Dr.", criteria.Name);
            Assert.Equal("Medicine", criteria.Specialty);
            Assert.Equal(1, criteria.PageNumber);
            Assert.Equal(10, criteria.PageSize);
            Assert.Equal("Name", criteria.SortBy);
            Assert.False(criteria.SortDescending);
        }

        [Fact]
        public void All_Sample_Providers_Should_Have_Required_Fields()
        {
            // Act
            var providers = ProviderTestDataBuilder.GetSampleProviders();

            // Assert
            foreach (var provider in providers)
            {
                Assert.False(string.IsNullOrEmpty(provider.ProviderNumber));
                Assert.False(string.IsNullOrEmpty(provider.Name));
                Assert.False(string.IsNullOrEmpty(provider.NPI));
                Assert.False(string.IsNullOrEmpty(provider.Address));
                Assert.False(string.IsNullOrEmpty(provider.Phone));
                Assert.False(string.IsNullOrEmpty(provider.Email));
                Assert.False(string.IsNullOrEmpty(provider.Specialty));
                
                // Verify NPI is 10 digits
                Assert.Equal(10, provider.NPI.Length);
                Assert.True(provider.NPI.All(char.IsDigit));
                
                // Verify email contains @
                Assert.Contains("@", provider.Email);
            }
        }
    }
}