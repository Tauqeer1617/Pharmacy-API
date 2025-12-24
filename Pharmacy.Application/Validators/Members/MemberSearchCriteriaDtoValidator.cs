using FluentValidation;
using Pharmacy.Application.DTOs.Members;

namespace Pharmacy.Application.Validators.Members
{
    public class MemberSearchCriteriaDtoValidator : AbstractValidator<MemberSearchCriteriaDto>
    {
        public MemberSearchCriteriaDtoValidator()
        {
            RuleFor(x => x.MemberNumber)
                .MaximumLength(50)
                .WithMessage("Member number must not exceed 50 characters.")
                .Matches("^[A-Za-z0-9-]*$")
                .WithMessage("Member number can only contain alphanumeric characters and hyphens.")
                .When(x => !string.IsNullOrEmpty(x.MemberNumber));

            RuleFor(x => x.FirstName)
                .MaximumLength(100)
                .WithMessage("First name must not exceed 100 characters.")
                .Matches("^[A-Za-z\\s]*$")
                .WithMessage("First name can only contain letters and spaces.")
                .When(x => !string.IsNullOrEmpty(x.FirstName));

            RuleFor(x => x.LastName)
                .MaximumLength(100)
                .WithMessage("Last name must not exceed 100 characters.")
                .Matches("^[A-Za-z\\s]*$")
                .WithMessage("Last name can only contain letters and spaces.")
                .When(x => !string.IsNullOrEmpty(x.LastName));

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

            RuleFor(x => x.Gender)
                .Must(BeValidGender)
                .WithMessage("Gender must be either 'Male', 'Female', or 'Other'.")
                .When(x => !string.IsNullOrEmpty(x.Gender));

            RuleFor(x => x.Address)
                .MaximumLength(500)
                .WithMessage("Address must not exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.Address));

            RuleFor(x => x.DateOfBirthFrom)
                .LessThanOrEqualTo(DateTime.Today)
                .WithMessage("Date of birth from cannot be in the future.")
                .When(x => x.DateOfBirthFrom.HasValue);

            RuleFor(x => x.DateOfBirthTo)
                .LessThanOrEqualTo(DateTime.Today)
                .WithMessage("Date of birth to cannot be in the future.")
                .GreaterThanOrEqualTo(x => x.DateOfBirthFrom)
                .WithMessage("Date of birth to must be greater than or equal to date of birth from.")
                .When(x => x.DateOfBirthTo.HasValue);

            RuleFor(x => x.AgeFrom)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Age from must be greater than or equal to 0.")
                .LessThanOrEqualTo(150)
                .WithMessage("Age from must be less than or equal to 150.")
                .When(x => x.AgeFrom.HasValue);

            RuleFor(x => x.AgeTo)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Age to must be greater than or equal to 0.")
                .LessThanOrEqualTo(150)
                .WithMessage("Age to must be less than or equal to 150.")
                .GreaterThanOrEqualTo(x => x.AgeFrom)
                .WithMessage("Age to must be greater than or equal to age from.")
                .When(x => x.AgeTo.HasValue);

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
                .WithMessage("Invalid sort field. Valid fields are: Id, MemberNumber, FirstName, LastName, DOB, Gender, Email, Phone.")
                .When(x => !string.IsNullOrEmpty(x.SortBy));
        }

        private bool BeValidGender(string? gender)
        {
            if (string.IsNullOrEmpty(gender))
                return true;

            var validGenders = new[] { "Male", "Female", "Other" };
            return validGenders.Contains(gender, StringComparer.OrdinalIgnoreCase);
        }

        private bool BeValidSortField(string? sortBy)
        {
            if (string.IsNullOrEmpty(sortBy))
                return true;

            var validSortFields = new[] { "Id", "MemberNumber", "FirstName", "LastName", "DOB", "Gender", "Email", "Phone", "Address" };
            return validSortFields.Contains(sortBy, StringComparer.OrdinalIgnoreCase);
        }
    }
}