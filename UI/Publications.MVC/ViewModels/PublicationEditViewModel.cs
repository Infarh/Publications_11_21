using System.ComponentModel.DataAnnotations;
using Publications.Domain.Entities;

namespace Publications.MVC.ViewModels
{
    public class PublicationEditViewModel
    {
        //[HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Аннотация")]
        public string Annotation { get; set; }

        [Display(Name = "Дата")]
        public DateTime Date { get; set; }

        public string Authors { get; set; }

        public string Place { get; set; }
    }
}
