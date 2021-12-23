using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Publications.MVC.ViewModels;

public class AuthorViewModel : IValidatableObject
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Display(Name = "Фамилия")]
    [Required(ErrorMessage = "Обязательно")]
    [StringLength(255, MinimumLength = 2, ErrorMessage = "Длина должна быть от 2 до 255 символов")]
    [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Все символы должны быть либо на русском, либо на английском. Первая буква - заглавная")]
    public string LastName { get; set; }

    [Display(Name = "Имя")]
    [Required(ErrorMessage = "Обязательно")]
    [StringLength(255, MinimumLength = 2, ErrorMessage = "Длина должна быть от 2 до 255 символов")]
    [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Все символы должны быть либо на русском, либо на английском. Первая буква - заглавная")]
    public string Name { get; set; }

    [Display(Name = "Отчество")]
    [StringLength(255, ErrorMessage = "Длина должна быть до 255 символов")]
    [RegularExpression(@"^([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)?$", ErrorMessage = "Все символы должны быть либо на русском, либо на английском. Первая буква - заглавная")]
    public string Patronymic { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext Context)
    {
        //yield return new ValidationResult("Информация об ошибке", new[] { nameof(LastName) });
        yield return ValidationResult.Success!;
    }
}