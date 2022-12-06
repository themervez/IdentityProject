using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityProject.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage ="Lütfen Kullanıcı Adı Giriniz.")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Display(Name = "Tel. No:")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "E-mail Adresi Gereklidir.")]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage ="E-mail Adresiniz Doğru Formatta Değil.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifreniz Gereklidir.")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
