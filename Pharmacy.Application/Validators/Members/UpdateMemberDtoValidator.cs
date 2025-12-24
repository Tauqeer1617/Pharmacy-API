using FluentValidation;
using Pharmacy.Application.DTOs.Members;

namespace Pharmacy.Application.Validators.Members
{
    public class UpdateMemberDtoValidator : AbstractValidator<UpdateMemberDto>
    {
        public UpdateMemberDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First name is required.")
                .MaximumLength(100)
                .WithMessage("First name must not exceed 100 characters.")
                .Matches("^[A-Za-z\\s]+$")
                .WithMessage("First name can only contain letters and spaces.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name is required.")
                .MaximumLength(100)
                .WithMessage("Last name must not exceed 100 characters.")
                .Matches("^[A-Za-z\\s]+$")
                .WithMessage("Last name can only contain letters and spaces.");

            RuleFor(x => x.DOB)
                .NotEmpty()
                .WithMessage("Date of birth is required.")
                .Must(BeAValidAge)
                .WithMessage("Date of birth must be between 1 and 120 years ago.");

            RuleFor(x => x.Gender)
                .NotEmpty()
                .WithMessage("Gender is required.")
                .Must(BeValidGender)
                .WithMessage("Gender must be either 'Male', 'Female', or 'Other'.");

            RuleFor(x => x.Address)
                .MaximumLength(500)
                .WithMessage("Address must not exceed 500 characters.");

            RuleFor(x => x.Phone)
                .MaximumLength(20)
                .WithMessage("Phone number must not exceed 20 characters.")
                .Matches("^[\\+]?[0-9\\s\\-\\(\\)]+$")
                .WithMessage("Phone number format is invalid.")
                .When(x => !string.IsNullOrEmpty(x.Phone));

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Email address format is invalid.")
                .MaximumLength(100)
                .WithMessage("Email must not exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.Email));
        }

        private bool BeAValidAge(DateTime dateOfBirth)
        {
            var age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age--;

            return age >= 1 && age <= 120;
        }

        private bool BeValidGender(string gender)
        {
            if (string.IsNullOrEmpty(gender))
                return false;

            var validGenders = new[] { "Male", "Female", "Other" };
            return validGenders.Contains(gender, StringComparer.OrdinalIgnoreCase);
        }
    }
}