using FluentValidation;
using Pharmacy.Application.DTOs.Providers;

namespace Pharmacy.Application.Validators.Providers
{
    public class ProviderSearchCriteriaDtoValidator : AbstractValidator<ProviderSearchCriteriaDto>
    {
        public ProviderSearchCriteriaDtoValidator()
        {
            RuleFor(x => x.ProviderNumber)
                .MaximumLength(50)
                .WithMessage("Provider number must not exceed 50 characters.")
                .Matches("^[A-Za-z0-9-]*$")
                .WithMessage("Provider number can only contain alphanumeric characters and hyphens.")
                .When(x => !string.IsNullOrEmpty(x.ProviderNumber));

            RuleFor(x => x.Name)
                .MaximumLength(200)
                .WithMessage("Provider name must not exceed 200 characters.")
                .Matches("^[A-Za-z\\s\\.,'-]*$")
                .WithMessage("Provider name contains invalid characters.")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.NPI)
                .MaximumLength(10)
                .WithMessage("NPI must not exceed 10 characters.")
                .Matches("^[0-9]*$")
                .WithMessage("NPI must contain only numeric characters.")
                .When(x => !string.IsNullOrEmpty(x.NPI));

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Invalid email format.")
                .MaximumLength(100)
                .WithMessage("Email must not exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Phone)
                .MaximumLength(20)
                .WithMessage("Phone number must not exceed 20 characters.")
                .Matches("^[\\+]?[0-9\\s\\-\\(\\)]*$")
                .WithMessage("Phone number format is invalid.")
                .When(x => !string.IsNullOrEmpty(x.Phone));

            RuleFor(x => x.Specialty)
                .MaximumLength(100)
                .WithMessage("Specialty must not exceed 100 characters.")
                .Matches("^[A-Za-z\\s\\.,'-]*$")
                .WithMessage("Specialty contains invalid characters.")
                .When(x => !string.IsNullOrEmpty(x.Specialty));

            RuleFor(x => x.Address)
                .MaximumLength(500)
                .WithMessage("Address must not exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.Address));

            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(100)
                .WithMessage("Page size must not exceed 100.");

            RuleFor(x => x.SortBy)
                .Must(BeValidSortField)
                .WithMessage("Invalid sort field. Valid fields are: Id, ProviderNumber, Name, NPI, Email, Phone, Specialty, Address.")
                .When(x => !string.IsNullOrEmpty(x.SortBy));
        }

        private bool BeValidSortField(string? sortBy)
        {
            if (string.IsNullOrEmpty(sortBy))
                return true;

            var validSortFields = new[] { "Id", "ProviderNumber", "Name", "NPI", "Email", "Phone", "Specialty", "Address" };
            return validSortFields.Contains(sortBy, StringComparer.OrdinalIgnoreCase);
        }
    }
}