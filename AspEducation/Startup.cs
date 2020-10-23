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
            //���������� ������ �� appsettings.json
            Configuration.Bind("Project", new Config());

            //���������� ������ ���������� � �������� ��������
            services.AddTransient<ITextFieldsRepository, EFTextFieldsRepository>();
            services.AddTransient<IServiceItemsRepository, EFServiceItemsRepository>();
            services.AddTransient<DataAdapter>();

            //���������� �������� � ��
            services.AddDbContext<AppDbContext>(x => x.UseSqlServer(Config.ConnectionString));

            //����������� identity �������
            services.AddIdentity<IdentityUser, IdentityRole>(o =>
           {
               o.User.RequireUniqueEmail = true;
               o.Password.RequiredLength = 6;
               o.Password.RequireNonAlphanumeric = false;
               o.Password.RequireLowercase = false;
               o.Password.RequireUppercase = false;
               o.Password.RequireDigit = false;
           }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            //����������� autentification cookie
            services.ConfigureApplicationCookie(o =>
            {
                o.Cookie.Name = "myCompanyAuth";
                o.Cookie.HttpOnly = true;
                o.LoginPath = "/account/accessdenied";
                o.SlidingExpiration = true;
            });

            //��������� ������� ��� ������������ � ������������� MVC
            services.AddControllersWithViews()
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0)
                .AddSessionStateTempDataProvider();

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //� �������� ���������� ��� ����� ������ ����� ������
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //���������� ���������� ��������� ������ (�������� �����)
            app.UseStaticFiles();

            //���������� ������� �������������
            app.UseRouting();

            //���������� �������������� � �����������
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            //������������ ������ ��� ��������
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}