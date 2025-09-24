using FluentValidation;
using TodoApp.Application.DTOs;

namespace TodoApp.Application.Validators
{
    /// <summary>
    /// CreateCategoryDto Validation Rules
    /// </summary>
    public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters.");
        }
    }
}