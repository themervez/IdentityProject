using IdentityProject.Models;
using IdentityProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            TempData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel userlogin)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByEmailAsync(userlogin.Email);
                if (user != null)
                {
                    if (await _userManager.IsLockedOutAsync(user))
                    {
                        ModelState.AddModelError("", "Hesabınız Bir Süreliğine Kilitlenmiştir. Lütfen Daha Sonra Tekrar Deneyiniz!");
                        return View(userlogin);
                    }

                    await _signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, userlogin.Password, userlogin.RememberMe, false);//Checkbox'tan alınan beni hatırla özelliğine göre Cookie'nin ömrünü belirledik

                    if (result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);
                        if (TempData["ReturnUrl"] != null)
                        {
                            return Redirect(TempData["ReturnUrl"].ToString());//Kullancının Login sayfasına gitmeden önceki sayfaya geri dönmesini sağladık
                        }
                        return RedirectToAction("Index", "Member");
                    }
                    else
                    {
                        await _userManager.AccessFailedAsync(user);//Başarısız erişim sayısını 1 arttıracak
                        int fail = await _userManager.GetAccessFailedCountAsync(user);//user'ın kaç başarısız giriş yaptığını aldık
                        ModelState.AddModelError("",$"{fail} Kez Başarısız Giriş.");
                        if (fail == 3)
                        {
                            await _userManager.SetLockoutEndDateAsync(user, new System.DateTimeOffset(DateTime.Now.AddMinutes(15)));
                            ModelState.AddModelError("", "3 Başarısız Giriş Nedeniyle Hesabınıza 15 Dakika Süreyle Erişim Reddedilmiştir! Lütfen Daha Sonra Tekrar Deneyiniz.");
                        }
                        else
                        {
                            ModelState.AddModelError("", "E-mail veya Şifreniz Yanlış!");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Bu E-mail Adresine Ait Kullanıcı Bulunamamıştır!");
                }
            }
            else
            {
            }
            return View(userlogin);//Hata varsa da eklenmiş Modeli yine gönderiyorum
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser();
                user.UserName = userViewModel.UserName;
                user.Email = userViewModel.Email;
                user.PhoneNumber = userViewModel.PhoneNumber;

                IdentityResult result = await _userManager.CreateAsync(user, userViewModel.Password);//Error keyword'de hatalarımızı yakalayabilmek için 
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (IdentityError item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);//Modele hataları ekledik
                    }
                }
            }
            return View(userViewModel);//Kullanıcıya Modeli hatalarla beraber gönderdik; eğer hata mesajı varsa
        }
    }
}
