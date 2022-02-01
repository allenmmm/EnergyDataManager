using EnergyDataManager.Data;
using EnergyDataManager.Domain.Interfaces;
using EnergyDataManager.SharedKernel.interfaces;
using EnergyDataManager.SharedKernel.Interfaces;
using EnergyDataManager.Web.DomainServices;
using EnergyDataManager.Web.Filters;
using EnergyDataReader.Account.File;
using EnergyDataReader.File;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedKernel;
using SharedKernel.Interfaces;

namespace EnergyDataManager.Web
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
            services.AddScoped<IEDMRepo, EDMRepo>();
            services.AddDbContext<EDMContext>();
            services.AddScoped<MeterReadingFile,MeterReadingFile>();
            services.AddScoped<IFileReadService<SourceMeterReading, string>, 
                FileReadService<SourceMeterReading>>();
            services.AddSingleton<
                IConverterList<Domain.Account, SourceMeterReading>,
                Account_SourceMeterReading_Converter>();
            services.AddSingleton<
                IConverter<SourceMeterReading, string>,
                MeterReadingFileConverter>();
            services.AddSingleton<
                IConverter<EnergyDataReader.Account.File.Account, string>,
                AccountFileConverter>();

            services.AddControllers(
                 options => options.Filters.Add(
                     new ExceptionFilter())).AddJsonOptions(options =>
                     {
                         options.JsonSerializerOptions.PropertyNamingPolicy = null;
                         options.JsonSerializerOptions.IgnoreNullValues = true;
                     }); ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
