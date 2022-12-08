using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityProject.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name ="E-mail")]
        [Required(ErrorMessage ="Email alanı gereklidir!")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre gereklidir!")]
        [DataType(DataType.Password)]
        [MinLength(4,ErrorMessage ="Şifre en az 4 karakter içermelidir!")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
