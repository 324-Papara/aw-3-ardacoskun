using FluentValidation;
using Para.Data.Domain;

namespace Para.Bussiness.Validations
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First Name is required.")
                .Length(3, 30)
                .WithMessage("Firstname must be between 3 and 30 characters in length!");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last Name is required.")
                .Length(3, 30)
                .WithMessage("Lastname must be between 3 and 30 characters in length!");

            RuleFor(x => x.IdentityNumber)
                .NotEmpty()
                .WithMessage("Identity number is required.")
                .Length(11)
                .WithMessage("Identity number must be 11 characters long!");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.");


            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .WithMessage("Date of birth is required")
                .LessThan(DateTime.Now)
                .WithMessage("Date of birth must be before today.");

            RuleFor(x => x.CustomerNumber)
                .NotEmpty()
                .WithMessage("Customer number is required.")
                .InclusiveBetween(1, 1000)
                .WithMessage("Customer number must be between 1-1000!");
        }
    }
}
