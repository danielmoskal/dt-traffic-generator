﻿using Data.Models;
using Data.Repositories.Concrete;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestLibrary.Infrastructure.RunTest.Abstract;
using TestLibrary.Infrastructure.RunTest.Concrete;
using Swashbuckle.AspNetCore.Swagger;
using TestLibrary.Infrastructure.ObjectsConverter.Abstract;
using TestLibrary.Infrastructure.ObjectsConverter.Concrete;
using TestLibrary.Repositories.Abstract;
using TestLibrary.Providers.Abstract;
using TestLibrary.Providers.Concrete;

namespace API
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

            services.AddTransient<EfficiencyTestDbContext>();
            services.AddTransient<IEndpointRepository, EndpointRepository>();
            services.AddTransient<ITestParametersRepository, TestParametersRepository>();
            services.AddDbContext<EfficiencyTestDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("EfficiencyTestDatabase")));
            services.AddTransient<ITestRunner, TestRunner>();
            services.AddTransient<ITestParametersProvider, TestParametersProvider>();
            services.AddTransient<IDataToBusinessObjectsConverter, DataToBusinessObjectsConverter>();
          
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Info { Title = "dt-traffic-generator Api", Description = "DayTrader - Traffic Generation - Swagger Api Documentation" });
            });
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
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "dt-traffic-generator Api");
            });
        }
    }
}
