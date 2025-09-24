using System;
using FluentValidation;
using TodoApp.Application.DTOs;

namespace TodoApp.Application.Validators
{
    /// <summary>
    /// CreateTodoDto Validation Rules
    /// FluentValidation kullanarak CreateTodoDto için doğrulama kuralları tanımlar
    /// </summary>
    public class CreateTodoDtoValidator : AbstractValidator<CreateTodoDto>
    {
        public CreateTodoDtoValidator()
        {
            // Title alanı
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

            // Description alanı
            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

            // Priority alanı
            RuleFor(x => x.Priority)
                .InclusiveBetween(1, 3).WithMessage("Priority must be between 1 and 3.");

            // DueDate alanı - dinamik kontrol
            RuleFor(x => x.DueDate)
                .Must(d => d == null || d > DateTime.UtcNow)
                .WithMessage("Due date must be in the future.");

            // CategoryId alanı (opsiyonel, null olabilir; verilirse en az 1 olmalı)
            RuleFor(x => x.CategoryId)
                .Must(id => id == null || id >= 1)
                .WithMessage("CategoryId must be null or greater than or equal to 1.");
        }
    }
}
