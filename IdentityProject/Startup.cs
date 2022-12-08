using IdentityProject.CustomValidation;
using IdentityProject.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityProject
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppIdentityDbContext>(opts =>
            {
                opts.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString"));
            });

            services.AddIdentity<AppUser, AppRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;//1 den fazla ayný email ile kaydý önlemek için
                opts.User.AllowedUserNameCharacters = "abcçdefgðhýijklmnoöpqrsþtuüvwxyzABCÇDEFGHIÝJKLMNOÖPQRSÞTUVWXYZ0123456789-._";//UserName alanýnda kullanýlabilecek karakterleri de burada belirleyebiliyoruz,default olarak ingilizce karakter gelmekte

                opts.Password.RequiredLength = 4;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireDigit = false;
            }).AddPasswordValidator<CustomPasswordValidator>().AddUserValidator<CustomUserValidator>().AddErrorDescriber<CustomIdentityErrorDescriber>().AddEntityFrameworkStores<AppIdentityDbContext>();//Password Validation ekledik//User Validation ekledik//IdentityErrorDescriber ekledik

            CookieBuilder cookieBuilder = new CookieBuilder();
            cookieBuilder.Name = "IdentitySite";
            cookieBuilder.HttpOnly = false;//Client tarafýnda Cookie okumayý engellemek için
            cookieBuilder.SameSite = SameSiteMode.Strict;//Sadece Name kýsmýnda belirtilensite üzerinden cookilere ulaþabilmek için//Lax:bu özelliði kapatýr,Strict: bu özelliði kýsar.CSRF'i önlemek için.
                                                         //Bankacýlýk uygulamalarýna yönelik olmasý için Strict kullandým.
            cookieBuilder.SecurePolicy = CookieSecurePolicy.SameAsRequest;

            services.ConfigureApplicationCookie(opts =>
            {
                opts.ExpireTimeSpan = System.TimeSpan.FromDays(2);//Cooki'lerin kullanýcýnýn bilgisayarýnda kalma süresi
                opts.LoginPath = new PathString("/Home/Login");//Authorization gerekli sayfalar için yönlendirme
                opts.Cookie = cookieBuilder;
                opts.SlidingExpiration = true;//Cookie ömrünün yarýsýnda kullanýcý eðer yeniden istek atarsa cookie ömrünü uzatacak
            });
            services.AddMvc(option => option.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Middlewares
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();//JavaScript,Css gibi dosyalarýn yüklenmesi için
            app.UseRouting();
            app.UseAuthentication();//Ýlgili MVC'ye gelmeden authentication iþleminin çalýþmasýný istediðim için middleware'i Mvc ile ilgili Middleware'dan önce aldým
            app.UseMvcWithDefaultRoute();//Controller/Action/{id} route'unu da ekler
        }
    }
}
