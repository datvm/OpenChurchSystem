using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtSharp.AspNetCore;
using LukeVo.Ocms.Api.Models;
using LukeVo.Ocms.Api.Models.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceSharp.AspNetCore;

namespace LukeVo.Ocms.Api
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var appSettings = new AppSettings();
            this.Configuration.Bind(appSettings);
            services.AddSingleton(appSettings);

            services.AddJwtIssuerAndBearer(options =>
            {
                options.Audience = appSettings.Jwt.Audience;
                options.Issuer = appSettings.Jwt.Issuer;
                options.SecurityKey = appSettings.Jwt.SecurityKey;
                options.ExpireSeconds = appSettings.Jwt.TokenLifetime;
            });
            
            var sqlConnectionString = this.Configuration.GetConnectionString("OcmsEntities");
            services.AddDbContext<OcmsContext>(options =>
            {
                options.UseSqlServer(sqlConnectionString);
            });

            services.AddServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
