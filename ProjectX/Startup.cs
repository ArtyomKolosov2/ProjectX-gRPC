using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectX.BusinessLayer.GrpcServices;
using ProjectX.BusinessLayer.Services;

namespace ProjectX
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// IConfiguration property
        /// </summary>
        public IConfiguration Configuration { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            
            /*services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o => {
                    var validator = new JwtTokenValidatorService(Configuration);
                    o.SecurityTokenValidators.Add(validator);
                });
            
            services.AddAuthorization();
            */
            
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
 
            /*app.UseAuthentication();
            app.UseAuthorization();*/

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GreeterService>();
            });
        }
    }
}