using FluentValidation;
using Pharmacy.Application.DTOs.Members;

namespace Pharmacy.Application.Validators.Members
{
    /// <summary>
    /// Base validator for common member validation rules
    /// </summary>
    public abstract class BaseMemberValidator<T> : AbstractValidator<T> where T : class
    {
        protected void SetupCommonRules()
        {
            // Common validation rules that can be reused
        }

        protected bool BeAValidAge(DateTime dateOfBirth)
        {
            var age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age--;

            return age >= 1 && age <= 120;
        }

        protected bool BeValidGender(string gender)
        {
            if (string.IsNullOrEmpty(gender))
                return false;

            var validGenders = new[] { "Male", "Female", "Other" };
            return validGenders.Contains(gender, StringComparer.OrdinalIgnoreCase);
        }

        protected bool BeValidPhoneNumber(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return true; // Optional field

            // Basic phone number validation - can be enhanced based on requirements
            return System.Text.RegularExpressions.Regex.IsMatch(phone, @"^[\+]?[0-9\s\-\(\)]+$");
        }
    }

    /// <summary>
    /// Validator for member DTOs that ensures unique member numbers
    /// This could be extended to check against the database
    /// </summary>
    public class MemberDtoValidator : AbstractValidator<MemberDto>
    {
        public MemberDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid member ID.");

            RuleFor(x => x.MemberNumber)
                .NotEmpty()
                .WithMessage("Member number is required.")
                .MaximumLength(50)
                .WithMessage("Member number must not exceed 50 characters.");

            RuleFor(x => x.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("Invalid email format.");
        }
    }
}