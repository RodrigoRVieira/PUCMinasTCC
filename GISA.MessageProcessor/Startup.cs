using GISA.Domain.Repository;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using SAF.Repository;

using System;

namespace GISA.MessageProcessor
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
            #region EfCore

            services.AddDbContext<SAFDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("ConnectionString")));

#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
            services.BuildServiceProvider().GetService<SAFDbContext>().Database.EnsureCreated();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'

            #endregion

            #region Repositories

            services.AddScoped<IAssociadoRepository, AssociadoRepository>();
            services.AddScoped<IConsultaRepository, ConsultaRepository>();
            services.AddScoped<IEspecialidadeRepository, EspecialidadeRepository>();
            services.AddScoped<IPrestadorRepository, PrestadorRepository>();

            #endregion

            var daprHttpPort = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3601";
            var daprGrpcPort = Environment.GetEnvironmentVariable("DAPR_GRPC_PORT") ?? "60001";
            services.AddDaprClient(builder => builder
                .UseHttpEndpoint($"http://localhost:{daprHttpPort}")
                .UseGrpcEndpoint($"http://localhost:{daprGrpcPort}"));

            services.AddControllers().AddDapr();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCloudEvents();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapSubscribeHandler();
                endpoints.MapControllers();
            });
        }
    }
}
