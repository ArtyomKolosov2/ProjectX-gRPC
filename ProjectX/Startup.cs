using Calzolari.Grpc.AspNetCore.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ProjectX.BusinessLayer.GrpcServices;
using ProjectX.BusinessLayer.Services;
using ProjectX.BusinessLayer.Services.DI;
using ProjectX.DataAccess.Context.DI;
using ProjectX.DataAccess.Models;
using ProjectX.DataAccess.Models.Base;
using ProjectX.DataAccess.Models.DI;
using ProjectX.DataAccess.Repositories.DI;

namespace ProjectX
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual void ConfigureDb(IServiceCollection services)
        {
            var databaseSettings = new DatabaseSettings();
            Configuration.GetSection(nameof(DatabaseSettings)).Bind(databaseSettings);

            services.AddSingleton<IDatabaseSettings>(sp => sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);
            
            services.AddMongoDbContext(databaseSettings);
            services.AddMongoDbIdentity(databaseSettings);
        }
        
        public virtual void ConfigureServices(IServiceCollection services)
        {
            ConfigureDb(services);
            
            services.AddGrpc(options =>
            {
                options.EnableMessageValidation();
                options.EnableDetailedErrors = true;
            });

            services.AddGrpcValidation();
            services.AddBusinessServices();
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    var validator = new JwtTokenValidatorService(Configuration);
                    x.SecurityTokenValidators.Add(validator);
                });

            services.AddAuthorization();
            services.AddRepositories();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<AuthenticationService>();
                endpoints.MapGrpcService<GreeterService>();
                endpoints.MapGrpcService<FileUploaderService>();
            });
        }
    }
}