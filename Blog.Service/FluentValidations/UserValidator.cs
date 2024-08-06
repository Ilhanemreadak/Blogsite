using Blog.Entity.Entities;
using FluentValidation;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service.FluentValidations
{
    public class UserValidator : AbstractValidator<AppUser>
    {
       public UserValidator() 
        {
            RuleFor(x => x.FirstName)
                .NotNull()
                .MinimumLength(2)
                .MaximumLength(50)
                .WithName("İsim");

            RuleFor(x => x.LastName)
                .NotNull()
                .MinimumLength(2)
                .MaximumLength(50)
                .WithName("Soyisim");

            RuleFor(x => x.PhoneNumber)
                .NotNull()
                .MinimumLength(11)
                .MaximumLength(50)
                .WithName("Telefon numarası");
        }
    }
}
