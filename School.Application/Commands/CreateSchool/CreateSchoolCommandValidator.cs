using FluentValidation;

namespace School.Application.Commands.CreateSchool;

public class CreateSchoolCommandValidator
    : AbstractValidator<CreateSchoolCommand>
{
    public CreateSchoolCommandValidator()
    {
        RuleFor(x => x.Dto.Name)
            .NotEmpty();

        RuleFor(x => x.Dto.Code)
            .NotEmpty()
            .Length(3, 10);

        RuleFor(x => x.Dto.City)
            .NotEmpty();

        RuleFor(x => x.Dto.Country)
            .NotEmpty();

        RuleFor(x => x.Dto.PostalCode)
           .NotEmpty()
           .Length(6);
    }
}
