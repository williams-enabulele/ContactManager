using ContactManager.Model.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Common.Validation
{
    public class RegisterDTOValidation : AbstractValidator<RegisterRequestDTO>
    {
        public RegisterDTOValidation()
        {
            RuleFor(user => user.Email).EmailAddress().WithMessage("Not A valid Email");
            RuleFor(user => user.Password).NotEmpty().WithMessage("Password cannot be empty")
                .NotNull().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain a number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password ust contain non alphanumeric");
            RuleFor(user => user.FirstName).NotEmpty().WithMessage("FirstName Cannot be empty")
                .NotNull().WithMessage("FirstName is required")
                .MinimumLength(3).WithMessage("FirstName must be at leasr 3 characters")
                .Matches("[A-Za-z]").WithMessage("Numeric values in names are not allowed");
            RuleFor(user => user.LastName).NotEmpty().WithMessage("FirstName Cannot be empty")
               .NotNull().WithMessage("FirstName is required")
               .MinimumLength(3).WithMessage("FirstName must be at leasr 3 characters")
               .Matches("[A-Za-z]").WithMessage("Numeric values in names are not allowed");


        }
    }
}
