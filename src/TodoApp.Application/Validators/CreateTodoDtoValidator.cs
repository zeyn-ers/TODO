using FluentValidation;
using TodoApp.Application.DTOs;

namespace TodoApp.Application.Validators;

/// <summary>
/// CreateTodoDto Validation Rules
/// FluentValidation kullanarak CreateTodoDto için doğrulama kuralları tanımlar
/// Bu validator, API'ye gelen verilerin doğruluğunu kontrol eder
/// </summary>
public class CreateTodoDtoValidator : AbstractValidator<CreateTodoDto>
{
    /// <summary>
    /// Constructor - Validation kurallarını tanımlar
    /// </summary>
    public CreateTodoDtoValidator()
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

        // DueDate alanı için kurallar
        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future.") // Gelecek tarih olmalı
            .When(x => x.DueDate.HasValue); // Sadece değer varsa kontrol et
    }
}
