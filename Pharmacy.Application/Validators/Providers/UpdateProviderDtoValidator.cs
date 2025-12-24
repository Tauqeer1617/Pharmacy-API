using FluentValidation;
using Pharmacy.Application.DTOs.Providers;

namespace Pharmacy.Application.Validators.Providers
{
    public class UpdateProviderDtoValidator : AbstractValidator<UpdateProviderDto>
    {
        public UpdateProviderDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Provider name is required.")
                .MaximumLength(200)
                .WithMessage("Provider name must not exceed 200 characters.")
                .Matches("^[A-Za-z\\s\\.,'-]*$")
                .WithMessage("Provider name contains invalid characters.");

            RuleFor(x => x.NPI)
                .NotEmpty()
                .WithMessage("NPI is required.")
                .Length(10)
                .WithMessage("NPI must be exactly 10 digits.")
                .Matches("^[0-9]{10}$")
                .WithMessage("NPI must contain only numeric characters.");

            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Address is required.")
                .MaximumLength(500)
                .WithMessage("Address must not exceed 500 characters.");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("Phone number is required.")
                .MaximumLength(20)
                .WithMessage("Phone number must not exceed 20 characters.")
                .Matches("^[\\+]?[0-9\\s\\-\\(\\)]*$")
                .WithMessage("Phone number format is invalid.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.")
                .MaximumLength(100)
                .WithMessage("Email must not exceed 100 characters.");

            RuleFor(x => x.Specialty)
                .NotEmpty()
                .WithMessage("Specialty is required.")
                .MaximumLength(100)
                .WithMessage("Specialty must not exceed 100 characters.")
                .Matches("^[A-Za-z\\s\\.,'-]*$")
                .WithMessage("Specialty contains invalid characters.");
        }
    }
}