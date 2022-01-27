using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProEventos.API.Helpers;
using ProEventos.Application.Services;
using ProEventos.Application.Services.Interfaces;
using ProEventos.Domain.Identity;
using ProEventos.Persistence;
using ProEventos.Persistence.Repository;
using ProEventos.Persistence.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;

namespace ProEventosAPI
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
            services.AddDbContext<ProEventosContext>(context => 
                context.UseSqlite(Configuration.GetConnectionString("Default"))
            );

            services.AddIdentityCore<User>( cfg => 
                {
                    cfg.Password.RequireDigit = false;
                    cfg.Password.RequireNonAlphanumeric = false;
                    cfg.Password.RequireLowercase = false;
                    cfg.Password.RequireUppercase = false;
                    cfg.Password.RequiredLength = 6;
                } 
            ).AddRoles<Role>()
             .AddRoleManager<RoleManager<Role>>()
             .AddSignInManager<SignInManager<User>>()
             .AddRoleValidator<RoleValidator<Role>>()
             .AddEntityFrameworkStores<ProEventosContext>()
             .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(cfg => 
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"])),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            
            services.AddControllers()
                .AddJsonOptions(cfg => 
                cfg.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
                )
                .AddNewtonsoftJson(cfg => 
                cfg.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IEventService, EventSevice>();
            services.AddScoped<IPartService, PartSevice>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISpeakerService, SpeakerService>();
            services.AddScoped<ISocialMediaService, SocialMediaService>();

            services.AddScoped<IUtility, GeneralUtility>();

            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IGeneralRepository, GeneralRepository>();
            services.AddScoped<IPartRepository, PartRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISpeakerRepository, SpeakerRepository>();
            services.AddScoped<ISocialMediaRepository, SocialMediaRepository>();


            services.AddCors();

            services.AddSwaggerGen(cfg =>
            {
                cfg.SwaggerDoc("v1", new OpenApiInfo { Title = "ProEventosAPI", Version = "v1" });
                cfg.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization Header usando Bearer.
                                    Entre com 'Bearer' + [espaço] + token.
                                    Exemplo: 'Bearer meutoken123456'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                cfg.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type  = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
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
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProEventosAPI - v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(c => 
            c.AllowAnyHeader()
             .AllowAnyMethod()
             .AllowAnyOrigin()
            );

            app.UseStaticFiles(new StaticFileOptions() {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Resources")),
                RequestPath = new PathString("/Resources")
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
