using System;
using System.Data;
using AutoMapper;
using AutoMapper.Data;
using Hurace.Api.Dtos;
using Hurace.Api.Extensions;
using Hurace.Api.Filters;
using Hurace.Api.Hubs;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Models;
using Hurace.Core.Models;
using Hurace.Core.Validators;
using Hurace.Data;
using Hurace.Data.Ado;
using Hurace.Data.Ado.Managers;
using Hurace.Logic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SqlKata.Compilers;

namespace Hurace.Api
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
            string allowedOrigin = Configuration.GetSection("AllowedOrigin").Value;
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(allowedOrigin)
                    .AllowCredentials();
            }));

            services.AddSignalR();

            services.AddControllers(options =>
            {
                options.Filters.Add(new HttpResponseExceptionFilter());
            });

            // Register the Swagger services
            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Hurace.Api";
                    document.Info.Description = "Christoph's and Daniel's Core 3.0 Api for Hurace";
                    document.Info.TermsOfService = "None";
                };
            });

            // Auto Mapper configuration
            services.AddSingleton(ConfigureMapper());

            // Connection and provider configuration
            string connectionString = Configuration.GetSection("DbConnectionString").Value;
            string providerName = Configuration.GetSection("DbProviderName").Value;

            services.AddSingleton<IConnectionFactory>(new DefaultConnectionFactory(connectionString, providerName));
            services.AddSingleton<Compiler>(DbUtil.GetCompiler(providerName));
            services.AddScoped<AdoManager>();

            // Manager and valitor registration
            services.AddSingleton(new LocationValidator());
            services.AddSingleton(new RaceDataValidator());
            services.AddSingleton(new RaceValidator());
            services.AddSingleton(new SkierValidator());
            services.AddSingleton(new StartListValidator());

            services.AddScoped<ILocationManager, LocationManager>();
            services.AddScoped<IRaceDataManager, RaceDataManager>();
            services.AddScoped<IRaceManager, RaceManager>();
            services.AddScoped<ISkierManager, SkierManager>();
            services.AddScoped<IStartListManager, StartListManager>();

            // Logic registration
            services.AddScoped<ILocationLogic, LocationLogic>();
            services.AddScoped<IRaceDataLogic, RaceDataLogic>();
            services.AddScoped<IRaceLogic, RaceLogic>();
            services.AddScoped<ISkierLogic, SkierLogic>();
            services.AddScoped<IStartListLogic, StartListLogic>();
            services.AddScoped<IStatisticLogic, StatisticLogic>();
            services.AddScoped<ISeasonLogic, SeasonLogic>();
        }

        public static IMapper ConfigureMapper()
        {
            return new MapperConfiguration(cfg =>
            {
                // Database Mapping
                cfg.AddDataReaderMapping();
                cfg.CreateMap<IDataRecord, Race>();
                cfg.CreateMap<IDataRecord, Skier>();
                cfg.CreateMap<IDataRecord, RaceData>();
                cfg.CreateMap<IDataRecord, Location>();
                cfg.CreateMap<IDataRecord, StartList>();

                // Dto Mappings
                cfg.CreateMapBidirectional<Location, LocationDto>();
                cfg.CreateMapBidirectional<Race, RaceDto>();
                cfg.CreateMap<RaceData, RaceDataDto>()
                    .ForMember(dest => dest.TimeStamp, c => c.MapFrom(src => src.TimeStamp.ToString(@"mm\:ss\.fff")));
                cfg.CreateMap<RaceStatisticEntry, RaceStatisticEntryDto>()
                    .ForMember(dest => dest.Time, c => c.MapFrom(src => src.Time.ToString(@"mm\:ss\.fff")))
                    .ForMember(dest => dest.DeltaTimeLeadership, c => c.MapFrom(src => src.DeltaTimeLeadership.ToString(@"mm\:ss\.fff")));
                cfg.CreateMapBidirectional<Skier, SkierDto>();
                cfg.CreateMapBidirectional<StartList, StartListDto>();
                cfg.CreateMap<LiveStatistic, LiveStatisticDto>()
                    .ForMember(dest => dest.TotalTime, c => c.MapFrom(src => src.TotalTime != null ? ((DateTime)src.TotalTime).ToString(@"mm\:ss\.fff") : null));
                cfg.CreateMap<LiveRaceData, LiveRaceDataDto>()
                    .ForMember(dest => dest.TotalTime, c => c.MapFrom(src => src.TotalTime.ToString(@"mm\:ss\.fff")));
                cfg.CreateMap<Season, SeasonDto>();
            })
            .CreateMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<RaceHub>("/live");
            });
        }
    }
}
