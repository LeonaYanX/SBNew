using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SB.ViewModels
{
    public class BookVM
    {
        [Display(Name ="Айди")]
        public int Id { get; set; }
        [Display(Name = "Название")]
        public string? Title { get; set; }
        [Display(Name = "Автор")]
        public string? Author { get; set; }
        [Display(Name = "Цена")]
        public int? Price { get; set; }
        [Display(Name = "Обмен")]
        public bool Swap { get; set; } = false;
      
        public byte[] ? EditPhoto { get; set; }
        public string[] ?Src { get; set; }
        [Display(Name = "Категория")]
        public string Category { get; set; }
        [Display(Name = "Информация")]
        public string? Info { get; set; }
        //
        [Display(Name ="Почта")]
        public string Email{ get; set; }
        [Display(Name = "Телефон")]
        public string Telephone { get; set; }

    }
}
