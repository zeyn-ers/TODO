using FluentValidation;
using TodoApp.Application.DTOs;

namespace TodoApp.Application.Validators
{
    /// <summary>
    /// UpdateCategoryDto Validation Rules
    /// </summary>
    public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters.");
        }
    }
}