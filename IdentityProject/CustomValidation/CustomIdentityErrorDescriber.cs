using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityProject.CustomValidation
{
    public class CustomIdentityErrorDescriber:IdentityErrorDescriber
    {
        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError()
            {
                Code = "InvalidUserName",
                Description = $"{userName}, Kullanıcı adı olarak geçersizdir!"
            };
        }
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError()
            {
                Code = "DuplicateEmail",
                Description = $"{email} zaten kullanılmaktadır."
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError()
            {
                Code = "PasswordTooShort",
                Description = $"Şifreniz en az{length} karakter uzunluğunda olmalıdır."//length: Startup.cs 'de belirttiğimiz karakter uzunluğu
            };
        }
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError()
            {
                Code = "DuplicateUserName",
                Description = $"Bu Kullanıcı adı({userName}) zaten kullanılmaktadır."//length: Startup.cs 'de belirttiğimiz karakter uzunluğu
            };
        }

    }
}
