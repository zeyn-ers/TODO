using FluentValidation;
using TodoApp.Application.DTOs;

namespace TodoApp.Application.Validators;

public class UpdateTodoDtoValidator : AbstractValidator<UpdateTodoDto>
{
    public UpdateTodoDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(x => x.Priority)
            .InclusiveBetween(1, 3).WithMessage("Priority must be between 1 and 3.");

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future.")
            .When(x => x.DueDate.HasValue);
    }
}
