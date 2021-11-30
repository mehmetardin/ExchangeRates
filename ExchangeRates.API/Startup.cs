using ExchangeRates.Application.Dto;
using ExchangeRates.Application.Interfaces;
using ExchangeRates.Application.Services;
using ExchangeRates.Application.Validations;
using ExchangeRates.Domain.Interfaces;
using ExchangeRates.Persistence.ExternalServices.TrainlineCurrencyService;
using ExchangeRates.Persistence.ExternalServices.TrainlineCurrencyService.Interfaces;
using ExchangeRates.Persistence.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace ExchangeRates.API
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExchangeRates.API", Version = "v1" });
            });

            services.AddScoped<ICurrencyConvertService, CurrencyConvertService>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IValidator<CurrencyConvertRequest>, CurrencyConvertRequestValidator>();
            services.AddScoped<IExternalCurrencyService, ExternalCurrencyService>();
            services.AddHttpClient<IExternalCurrencyService, ExternalCurrencyService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/error");

            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExchangeRates.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
