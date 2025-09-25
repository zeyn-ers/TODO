using FluentValidation;
using TodoApp.Application.DTOs;

namespace TodoApp.Application.Validators;

/// <summary>
/// UpdateCategoryDto için validation kuralları
/// FluentValidation kullanarak güncelleme verilerini doğrular
/// </summary>
public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryDtoValidator()
    {
        /// <summary>
        /// Kategori adı zorunlu ve maksimum 100 karakter
        /// </summary>
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Kategori adı zorunludur")
            .MaximumLength(100).WithMessage("Kategori adı maksimum 100 karakter olabilir")
            .MinimumLength(2).WithMessage("Kategori adı minimum 2 karakter olmalıdır");

        /// <summary>
        /// Açıklama opsiyonel ama varsa maksimum 500 karakter
        /// </summary>
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Açıklama maksimum 500 karakter olabilir")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}