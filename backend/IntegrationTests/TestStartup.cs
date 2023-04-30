using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using OnlineTerrainGeneratorWebAPI.Logic;

namespace IntegrationTests
{
    class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
            Configuration["Secret"] = "0123456789123456";
        }

        public IConfiguration Configuration { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton<HeightMapLogic>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            env.ContentRootPath = Directory.GetCurrentDirectory();
        }
    }
}
