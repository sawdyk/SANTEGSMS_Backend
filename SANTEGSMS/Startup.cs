using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SANTEGSMS.DatabaseContext;
using SANTEGSMS.Helpers;
using SANTEGSMS.IRepos;
using SANTEGSMS.Repos;
using SANTEGSMS.Reusables;
using SANTEGSMS.Security.BasicAuth;
using SANTEGSMS.Services.Cloudinary;
using SANTEGSMS.Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            services.AddMvc().AddJsonOptions(options =>
            {
                //options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                //options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                //options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

            });


            //configure Basic Authentication 
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            //Database Context
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseMySql(Configuration.GetConnectionString("DefaultConnection")).EnableDetailedErrors();
                //opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            //------------------EMAIL AND CLOUDINARY SERVICE--------------------------------------------------
            var emailConfig = Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);

            var cloudinaryConfig = Configuration.GetSection("Cloudinary").Get<CloudinaryConfig>();
            services.AddSingleton(cloudinaryConfig);


            // -------------------------------------CONFIGURE DEPENDENCY INJECTION FOR APPLICATION SERVICES-------------------------------------------------------------
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IEmailRepo, EmailRepo>();


            services.AddScoped<ISchoolRepo, SchoolRepo>();
            services.AddScoped<ISystemDefaultRepo, SystemDefaultRepo>();
            services.AddScoped<ILocalGovtRepo, LocalGovtRepo>();
            services.AddScoped<IDistrictRepo, DistrictRepo>();
            services.AddScoped<ISchoolCampusRepo, SchoolCampusRepo>();
            services.AddScoped<ISchoolUsersRepo, SchoolUsersRepo>();
            services.AddScoped<ISchoolRolesRepo, SchoolRolesRepo>();
            services.AddScoped<IClassRepo, ClassRepo>();
            services.AddScoped<ISessionTermRepo, SessionTermRepo>();
            services.AddScoped<IParentRepo, ParentRepo>();
            services.AddScoped<IScoresConfigRepo, ScoresConfigRepo>();
            services.AddScoped<IStudentRepo, StudentRepo>();
            services.AddScoped<ISubjectRepo, SubjectRepo>();
            services.AddScoped<ITeacherRepo, TeacherRepo>();
            services.AddScoped<IAssignmentRepo, AssignmentRepo>();
            services.AddScoped<ILessonNoteRepo, LessonNoteRepo>();
            services.AddScoped<IExtraCurricularBehavioralScoresRepo, ExtraCurricularBehavioralScoreRepo>();
            services.AddScoped<IReportCardConfigurationRepo, ReportCardConfigurationRepo>();
            services.AddScoped<IScoreUploadRepo, ScoreUploadRepo>();
            services.AddScoped<IReportCardRepo, ReportCardRepo>();
            services.AddScoped<IReportCardDataGenerateRepo, ReportCardGenerateRepo>();
            services.AddScoped<ISuperAdminRepo, SuperAdminRepo>();
            services.AddScoped<IFileUploadRepo, FileUploadRepo>();

            services.AddScoped<ReportCardReUsables>();
            services.AddScoped<EmailTemplate>();
            services.AddScoped<ServerPath>();

            //Get the swagger value options
            var swaggerOpt = Configuration.GetSection("SwaggerOptions").Get<SwaggerOptions>();
            // Register Swagger  
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = swaggerOpt.Title,
                    Version = swaggerOpt.Version
                });

                c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            new string[] {}
                    }
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"./v1/swagger.json", "SANTEG SMS");
            });

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
