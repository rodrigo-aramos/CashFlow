using CashFlow.API.Extensions;
using CashFlow.Data.Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Text;

namespace CashFlow.API
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
            // Documentation
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CashFlow.API", Version = "v1" });
            });

            // Context
            string connectionString = Configuration.GetConnectionString("PostgreSQL-Local");
            if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("DATABASE_HOST")))
                connectionString = Configuration.GetConnectionString("PostgreSQL-Production")
                    .Replace("{{DatabaseHost}}", Environment.GetEnvironmentVariable("DATABASE_HOST"))
                    .Replace("{{DatabasePort}}", Environment.GetEnvironmentVariable("DATABASE_PORT"))
                    .Replace("{{DatabaseName}}", Environment.GetEnvironmentVariable("DATABASE_NAME"))
                    .Replace("{{DatabaseUser}}", Environment.GetEnvironmentVariable("DATABASE_USER"))
                    .Replace("{{DatabasePass}}", Environment.GetEnvironmentVariable("DATABASE_PASS"));

            services
                .AddDbContext<FinancialDbContext>(
                    options =>
                    {
                        options.UseNpgsql(
                            connectionString,
                            x => x.MigrationsAssembly("CashFlow.Infrastructure")
                        );

#if DEBUG
                        options.EnableSensitiveDataLogging(true);
#endif

                    });

            // Logging
#if DEBUG
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole()
                    .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information);
                loggingBuilder.AddDebug();
            });

            // CORS
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins(
                            "http://localhost:5047"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithExposedHeaders("Content-Disposition");
                });
            });
#else
            // CORS
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://corporate.com")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithExposedHeaders("Content-Disposition");
                });
            });
#endif

            // Controllers
            services.AddControllers();

            // Authentication
            string secret = Configuration.GetValue<string>("Secret");
            var key = Encoding.ASCII.GetBytes(secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddDependencies();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddApiVersioning(opt =>
                {
                    opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                    opt.AssumeDefaultVersionWhenUnspecified = true;
                    opt.ReportApiVersions = true;
                    opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                                    new HeaderApiVersionReader("x-api-version"),
                                                                    new MediaTypeApiVersionReader("x-api-version"));
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DeskBooking.API v1"));
            }

#if !DEBUG
            app.UseHttpsRedirection();
#endif

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            app.ApplyMigrations();
        }
    }
}