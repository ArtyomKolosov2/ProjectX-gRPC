using System;
using System.Text;
using AspNetCore.Identity.Mongo;
using Calzolari.Grpc.AspNetCore.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using ProjectX.BusinessLayer.GrpcServices;
using ProjectX.BusinessLayer.Services.DI;
using ProjectX.DataAccess.Context.DI;
using ProjectX.DataAccess.Models;
using ProjectX.DataAccess.Models.Base;
using ProjectX.DataAccess.Models.Identity;
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
        
        public void ConfigureServices(IServiceCollection services)
        {
            var databaseSettings = new DatabaseSettings();
            Configuration.GetSection(nameof(DatabaseSettings)).Bind(databaseSettings);

            services.AddSingleton<IDatabaseSettings>(sp => sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);
            
            services.AddMongoDbContext(databaseSettings);
            
            services.AddGrpc(options =>
            {
                options.EnableMessageValidation();
                options.EnableDetailedErrors = true;
            });

            services.AddGrpcValidation();

            services.AddIdentityMongoDbProvider<User, Role, ObjectId>(identityOptions =>
            {
                identityOptions.Password.RequiredLength = 6;
                identityOptions.Password.RequireLowercase = false;
                identityOptions.Password.RequireUppercase = false;
                identityOptions.Password.RequireNonAlphanumeric = false;
                identityOptions.Password.RequireDigit = true;
                
                identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                identityOptions.Lockout.MaxFailedAccessAttempts = 5;
                identityOptions.Lockout.AllowedForNewUsers = true;
                
                identityOptions.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                identityOptions.User.RequireUniqueEmail = true;
            }, mongoIdentityOptions =>
            {
                mongoIdentityOptions.ConnectionString =
                    $"{databaseSettings.ConnectionString}/{databaseSettings.DatabaseName}";
            });

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
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtTokenKey"])),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            /*services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.SaveToken = true;
                });*/
            
            services.AddAuthorization();
            services.AddRepositories();


            /*services.AddTransient<AuthHeadersInterceptor>();
            services.AddHttpContextAccessor();

            var httpClientBuilder = services.AddGrpcClient<GreeterService>(o =>
            {
                o.Address = new Uri("https://localhost:5001");
            });
            
            httpClientBuilder.AddInterceptor<AuthHeadersInterceptor>();              
            httpClientBuilder.ConfigureChannel(o => o.Credentials = ChannelCredentials.Insecure);*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
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
            });
        }
    }
}