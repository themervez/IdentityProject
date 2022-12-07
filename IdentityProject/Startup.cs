using IdentityProject.CustomValidation;
using IdentityProject.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            services.AddMvc(option => option.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Middlewares
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();//JavaScript,Css gibi dosyalarýn yüklenmesi için
            app.UseMvcWithDefaultRoute();//Controller/Action/{id} route'unu da ekler

            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
