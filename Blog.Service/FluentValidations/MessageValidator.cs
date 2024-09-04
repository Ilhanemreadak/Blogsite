using Blog.Entity.Entities;
using FluentValidation;

namespace Blog.Service.FluentValidations
{
    public class MessageValidator : AbstractValidator<ContactMessages>
    {
        public MessageValidator() {
            RuleFor(c => c.Name)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(70)
                .WithName("İsim");
            RuleFor(c => c.Email)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(70)
                .EmailAddress().WithMessage("Geçerli bir email adresi gerekli!");
            RuleFor(c => c.Message)
                .NotNull()
                .MinimumLength(10)
                .MaximumLength(500);

        }
    }
}
