using System.ComponentModel.DataAnnotations;

namespace SB.Models
{
    public class Register
    {
        [Display(Name = "Почта")]
        public string Email { get; set; }
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        // public byte[]? Photo { get; set; }
        [Display(Name = "Телефон")]
        public string Phone { get; set; }
        [Display(Name = "Город")]
        public string City { get; set; }
        [Display(Name = "Возраст")]
        public int Age { get; set; }

        
    }
}
