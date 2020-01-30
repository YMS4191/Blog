using Entities.Models;
using Entities.ProjectContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;
using Services.Contracts;
using UI.Areas.Admin.Utility;

namespace UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<BlogContext>(options => options
            .UseSqlServer(Configuration.GetConnectionString("default"), a => a.MigrationsAssembly("UI")));
            services.AddControllersWithViews();


            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IPostImageService, PostImageService>();
            services.AddTransient<IFileUploader, FileSystemFileUploader>();
             
             
            services.AddIdentity<BlogUser, IdentityRole>(options => {
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 7;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                // şifre minimum 7 haneden oluşacak ve en az 1 adet sayı, büyük harf, küçük harf ve özel karakter içerecek.
            })
                .AddEntityFrameworkStores<BlogContext>()
                .AddDefaultTokenProviders();

            //  varsayılan login path => /account/login
            services.ConfigureApplicationCookie(
               options =>
               {
                   options.LoginPath = "/admin/account/login";
                   options.AccessDeniedPath = "/admin/account/accessdenied";
               });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "admin",
                    pattern: "{area:exists}/{controller=home}/{action=index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
