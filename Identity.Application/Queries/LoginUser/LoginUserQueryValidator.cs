using FluentValidation;

namespace Identity.Application.Queries.LoginUser;

public class LoginUserQueryValidator
    : AbstractValidator<LoginUserQuery>
{
    public LoginUserQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
