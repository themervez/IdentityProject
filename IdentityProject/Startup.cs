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
                opts.User.RequireUniqueEmail = true;//1 den fazla ayn� email ile kayd� �nlemek i�in
                opts.User.AllowedUserNameCharacters = "abc�defg�h�ijklmno�pqrs�tu�vwxyzABC�DEFGHI�JKLMNO�PQRS�TUVWXYZ0123456789-._";//UserName alan�nda kullan�labilecek karakterleri de burada belirleyebiliyoruz,default olarak ingilizce karakter gelmekte

                opts.Password.RequiredLength = 4;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireDigit = false;
            }).AddPasswordValidator<CustomPasswordValidator>().AddUserValidator<CustomUserValidator>().AddErrorDescriber<CustomIdentityErrorDescriber>().AddEntityFrameworkStores<AppIdentityDbContext>();//Password Validation ekledik//User Validation ekledik//IdentityErrorDescriber ekledik

            CookieBuilder cookieBuilder = new CookieBuilder();
            cookieBuilder.Name = "IdentitySite";
            cookieBuilder.HttpOnly = false;//Client taraf�nda Cookie okumay� engellemek i�in
            cookieBuilder.SameSite = SameSiteMode.Strict;//Sadece Name k�sm�nda belirtilensite �zerinden cookilere ula�abilmek i�in//Lax:bu �zelli�i kapat�r,Strict: bu �zelli�i k�sar.CSRF'i �nlemek i�in.
                                                         //Bankac�l�k uygulamalar�na y�nelik olmas� i�in Strict kulland�m.
            cookieBuilder.SecurePolicy = CookieSecurePolicy.SameAsRequest;

            services.ConfigureApplicationCookie(opts =>
            {
                opts.ExpireTimeSpan = System.TimeSpan.FromDays(2);//Cooki'lerin kullan�c�n�n bilgisayar�nda kalma s�resi
                opts.LoginPath = new PathString("/Home/Login");//Authorization gerekli sayfalar i�in y�nlendirme
                opts.Cookie = cookieBuilder;
                opts.SlidingExpiration = true;//Cookie �mr�n�n yar�s�nda kullan�c� e�er yeniden istek atarsa cookie �mr�n� uzatacak
            });
            services.AddMvc(option => option.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Middlewares
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();//JavaScript,Css gibi dosyalar�n y�klenmesi i�in
            app.UseRouting();
            app.UseAuthentication();//�lgili MVC'ye gelmeden authentication i�leminin �al��mas�n� istedi�im i�in middleware'i Mvc ile ilgili Middleware'dan �nce ald�m
            app.UseMvcWithDefaultRoute();//Controller/Action/{id} route'unu da ekler
        }
    }
}
