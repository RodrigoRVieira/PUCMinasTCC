using GISA.Domain.Repository;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using SAF.Repository;

using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace SAF.WebApi
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
            #region Compression

            services.Configure<GzipCompressionProviderOptions>(
                options => options.Level = CompressionLevel.Optimal);

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            });

            #endregion

            #region EfCore

            services.AddDbContext<SAFDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("ConnectionString")));

#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
            services.BuildServiceProvider().GetService<SAFDbContext>().Database.EnsureCreated();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'

            #endregion

            #region Cache

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration =
                Environment.GetEnvironmentVariable("REDIS_HOST") ?? Configuration.GetConnectionString("GISA.Redis");
                options.InstanceName = "GISA.SAF.";
            });

            #endregion

            #region Repositories

            services.AddScoped<IConsultaRepository, ConsultaRepository>();
            services.AddScoped<IEspecialidadeRepository, EspecialidadeRepository>();
            services.AddScoped<IPrestadorRepository, PrestadorRepository>();

            #endregion

            #region Versioning

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            #endregion

            #region Dapr

            var daprHttpPort = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3600";
            var daprGrpcPort = Environment.GetEnvironmentVariable("DAPR_GRPC_PORT") ?? "60000";

            services.AddDaprClient(builder => builder
                .UseHttpEndpoint($"http://localhost:{daprHttpPort}")
                .UseGrpcEndpoint($"http://localhost:{daprGrpcPort}"));

            #endregion

            #region Swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Contact = new OpenApiContact
                        {
                            Name = "PUC Minas - TCC"
                        },
                        Description = "GISA Rest API",
                        Title = "GISA - Gestao Integral da Saude do Associado",
                        Version = PlatformServices.Default.Application.ApplicationVersion
                    });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Scheme = "Bearer",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });

                c.IncludeXmlComments(
                    Path.Combine(
                        PlatformServices.Default.Application.ApplicationBasePath,
                        $"{PlatformServices.Default.Application.ApplicationName}.xml"));
            });

            #endregion

            #region Authentication/Authorization

            services.AddAuthentication(authenticationOptions =>
            {
                authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearerOptions =>
            {
                bearerOptions.RequireHttpsMetadata = false;
                bearerOptions.SaveToken = true;
                bearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(Configuration.GetSection("Authorization").GetValue<string>("Secret"))),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            #endregion

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                });

            #region Application Insights

            services.AddApplicationInsightsTelemetry(Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            #region Compression

            app.UseResponseCompression();

            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region Swagger

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GISA");
            });

            #endregion
        }
    }
}
