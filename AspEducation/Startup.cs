using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using AspEducation.Domain;
using AspEducation.Domain.Repositories.Abstract;
using AspEducation.Domain.Repositories.EntityFramework;
using AspEducation.Sevice;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspEducation
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            //Подключаем конфиг из appsettings.json
            Configuration.Bind("Project", new Config());

            //Подключаем нужный функционал в качестве сервисов
            services.AddTransient<ITextFieldsRepository, EFTextFieldsRepository>();
            services.AddTransient<IServiceItemsRepository, EFServiceItemsRepository>();
            services.AddTransient<DataAdapter>();

            //Подключаем контекст к БД
            services.AddDbContext<AppDbContext>(x => x.UseSqlServer(Config.ConnectionString));

            //Настраиваем identity систему
            services.AddIdentity<IdentityUser, IdentityRole>(o =>
           {
               o.User.RequireUniqueEmail = true;
               o.Password.RequiredLength = 6;
               o.Password.RequireNonAlphanumeric = false;
               o.Password.RequireLowercase = false;
               o.Password.RequireUppercase = false;
               o.Password.RequireDigit = false;
           }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            //Настраиваем autentification cookie
            services.ConfigureApplicationCookie(o =>
            {
                o.Cookie.Name = "myCompanyAuth";
                o.Cookie.HttpOnly = true;
                o.LoginPath = "/account/accessdenied";
                o.SlidingExpiration = true;
            });

            //Добавляем сервисы для контроллеров и представлений MVC
            services.AddControllersWithViews()
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0)
                .AddSessionStateTempDataProvider();

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //В процессе разработки нам важно видеть какие ошибки
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Подключаем поддержжку статичных файлов (ресурсов сайта)
            app.UseStaticFiles();

            //Подключаем систему маршрутизации
            app.UseRouting();

            //Подключаем аутентификацию и авторизацию
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            //Регистрируем нужные нам маршруты
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}