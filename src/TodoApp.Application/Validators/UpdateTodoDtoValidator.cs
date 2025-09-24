using System;
using FluentValidation;
using TodoApp.Application.DTOs;

namespace TodoApp.Application.Validators
{
    /// <summary>
    /// UpdateTodoDto Validation Rules
    /// FluentValidation kullanarak UpdateTodoDto için doğrulama kuralları tanımlar
    /// Bu validator, API'ye gelen güncelleme verilerinin doğruluğunu kontrol eder
    /// </summary>
    public class UpdateTodoDtoValidator : AbstractValidator<UpdateTodoDto>
    {
        /// <summary>
        /// Constructor - Validation kurallarını tanımlar
        /// </summary>
        public UpdateTodoDtoValidator()
        {
            // Title alanı için kurallar
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.") // Boş olamaz
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters."); // Maksimum 200 karakter

            // Description alanı için kurallar
            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters."); // Maksimum 1000 karakter

            // Priority alanı için kurallar
            RuleFor(x => x.Priority)
                .InclusiveBetween(1, 3).WithMessage("Priority must be between 1 and 3."); // 1-3 arası olmalı

            // DueDate alanı için kurallar - dinamik kontrol
            RuleFor(x => x.DueDate)
                .Must(d => d == null || d > DateTime.UtcNow)
                .WithMessage("Due date must be in the future."); // Gelecek tarih olmalı

            // CategoryId alanı (opsiyonel; verilirse en az 1)
            RuleFor(x => x.CategoryId)
                .Must(id => id == null || id >= 1)
                .WithMessage("CategoryId must be null or greater than or equal to 1.");
        }
    }
}
