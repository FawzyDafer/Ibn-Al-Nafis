using GUI.Models.Identity;
using GUI.Repository;
using GUI.Repository.Concrete;
using GUI.Repository.Service;
using GUI.Repository.Service.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GUI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IPasswordValidator<User>, CustomPasswordValidator>();
            services.AddTransient<IUserValidator<User>, CustomUserValidator>();
            services.AddTransient<IAuthorizationHandler, BlockUsersHandler>();
            services.AddTransient<IPatientRepository, PatientRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IAdmissionsRepository, AdmissionsRepository>();
            services.AddTransient<IClinicRepository, ClinicRepository>();
            services.AddTransient<IFollowUpRepository, FollowUpRepository>();
            services.AddTransient<IInvestigationRepository, InvestigationRepository>();
            services.AddTransient<ILogFileRepository, LogFileRepository>();
            services.AddTransient<IFAQRepository, FAQRepository>();
            services.AddTransient<IConsentsRepository, ConsentsRepository>();
            services.AddTransient<IDischargeRepository, DischargeRepository>();
            services.AddTransient<IPainAssissmentRepository, PainAssissmentRepository>();
            services.AddTransient<IImageRepository, ImageRepository>();
            services.AddTransient<IHistoryRepository, HistoryRepository>();
            services.AddTransient<IExaminationRepository, ExaminationRepository>();
            services.AddTransient<IMedicineRepository, MedicineRepository>();
            services.AddTransient<IInvestigationServices, InvestigationServices>();
            services.AddTransient<IReceptionService, ReceptionService>();
            services.AddTransient<IDoctorService, DoctorService>();
            services.AddTransient<IFAQServices, FAQServices>();
            services.AddTransient<ILogFileServices, LogFileServices>();
            services.AddTransient<IImageService, ImageService>();
            //Seading database with identity data 
            services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(Configuration["MHC:ConnectionString"]));
            //Password strength and validation
            services.AddIdentity<User, IdentityRole>(opts =>
            {
                opts.User.RequireUniqueEmail = false;
                opts.Password.RequiredLength = 8;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();
            //the pass to login in case of unauthorized user try to access the  resources
            services.ConfigureApplicationCookie(opts => opts.LoginPath = "/Home/Index");
            //This allow url to type is lower case
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            //Compatibility version of MVC
            services.AddMvc(options => options.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseStatusCodePages();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseBrowserLink();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: null,
                  template: "{area}/{controller}/{action}/{Page:int}",
                  defaults: new { area = "", controller = "Home", action = "Index" }
                );

                routes.MapRoute(
                  name: "Edit",
                  template: "{area}/{controller}/{action}/{uname}/{Search?}",
                  defaults: new { area = "", controller = "Home", action = "Index" }
                );

                routes.MapRoute(
                  name: null,
                  template: "{area}/{controller}/{action}/{uname?}",
                  defaults: new { area = "", controller = "Home", action = "Index" }
                );

                routes.MapRoute(name: null, template: "{controller}/{action}/{Page:int}/{Personid?}");

                routes.MapRoute(name: null, template: "{controller}/{action}/{Personid?}");
            });
        }

    }
}
