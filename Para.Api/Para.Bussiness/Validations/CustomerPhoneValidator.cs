using FluentValidation;
using Para.Data.Domain;

namespace Para.Bussiness.Validations
{
    public class CustomerPhoneValidator : AbstractValidator<CustomerPhone>
    {
        public CustomerPhoneValidator()
        {
            RuleFor(x => x.CountyCode)
                .NotEmpty()
                .WithMessage("Country code is required.")
                .MinimumLength(3)
                .WithMessage("Country Code must be at least 3 characters long!");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("Phone number is required.")
                .Length(11)
                .WithMessage("Phone number must be at least 11 characters long!");
        }
    }
}
