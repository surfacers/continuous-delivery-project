using System;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Data;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Models;
using Hurace.Core.Models;
using Hurace.Data;
using Hurace.Data.Ado;
using Hurace.Data.Ado.Managers;
using Hurace.Logic;
using Hurace.RaceControl.Services;
using Hurace.RaceControl.Services.Impl;
using Hurace.RaceControl.ViewModels.Race;
using Hurace.RaceControl.ViewModels.Race.Detail;
using Hurace.RaceControl.ViewModels.Skier;
using Hurace.Simulator;
using Hurace.Simulator.Models;
using Microsoft.AspNetCore.SignalR.Client;
using SqlKata.Compilers;
using Unity;

namespace Hurace.RaceControl
{
    public class StartupConfig
    {
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

                // Form Mappings
                cfg.CreateMap<Skier, SkierEditViewModel>()
                    .ForMember(dest => dest.Original, c => c.MapFrom(src => src));
                cfg.CreateMap<SkierEditViewModel, Skier>()
                    .ForMember(dest => dest.Id, c => c.MapFrom(src => src.Original.Id))
                    .ForMember(dest => dest.IsRemoved, c => c.MapFrom(src => src.Original.IsRemoved));

                cfg.CreateMap<Race, RaceEditViewModel>()
                    .ForMember(dest => dest.Original, c => c.MapFrom(src => src))
                    .ForMember(dest => dest.RaceType, c => c.Ignore())
                    .ForMember(dest => dest.LocationId, c => c.Ignore());
                cfg.CreateMap<RaceEditViewModel, Race>()
                    .ForMember(dest => dest.Id, c => c.MapFrom(src => src.Original.Id))
                    .ForMember(dest => dest.RaceType, c => c.MapFrom(src => src.RaceType.Value))
                    .ForMember(dest => dest.LocationId, c => c.MapFrom(src => src.LocationId.Value));

                cfg.CreateMap<ClockSettings, ClockSettingsEditViewModel>()
                    .ConstructUsing((_, __) => new ClockSettingsEditViewModel(App.Container.Resolve<IMapper>()));
                cfg.CreateMap<ClockSettingsEditViewModel, ClockSettings>();

                cfg.CreateMap<StartListItemViewModel, LiveStatistic>()
                    .ForMember(dest => dest.SkierId, c => c.MapFrom(src => src.Skier.Id))
                    .ForMember(dest => dest.FirstName, c => c.MapFrom(src => src.Skier.FirstName))
                    .ForMember(dest => dest.LastName, c => c.MapFrom(src => src.Skier.LastName))
                    .ForMember(dest => dest.CountryCode, c => c.MapFrom(src => src.Skier.CountryCode))
                    .ForMember(dest => dest.RaceId, c => c.MapFrom(src => src.StartList.RaceId))
                    .ForMember(dest => dest.RunNumber, c => c.MapFrom(src => src.StartList.RunNumber));
                cfg.CreateMap<RaceDataItemViewModel, LiveRaceData>();
            })
            .CreateMapper();
        }

        public static bool IsDebug()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }

        public static IUnityContainer ConfigureDependencies()
        {
            // Authenticate KeyVault Client
            var secrets = new SecretClient(
                new Uri($"https://{ConfigurationManager.AppSettings["KeyVaultName"]}.vault.azure.net/"),
                new DefaultAzureCredential());

            var container = new UnityContainer();

            // Services
            container.RegisterInstance<Hurace.Timer.IRaceClock>(null);
            container.RegisterSingleton<INotificationService, NotificationService>();
            container.RegisterSingleton<IClockService, ClockService>();

            // Logik
            container
                .RegisterType<ISkierLogic, SkierLogic>()
                .RegisterType<IRaceDataLogic, RaceDataLogic>()
                .RegisterType<IRaceLogic, RaceLogic>()
                .RegisterType<IStartListLogic, StartListLogic>()
                .RegisterType<ILocationLogic, LocationLogic>()
                .RegisterType<IRaceDataLogic, RaceDataLogic>()
                .RegisterType<IStatisticLogic, StatisticLogic>()
                .RegisterType<ISeasonLogic, SeasonLogic>();

            // Generic Manager
            string connectionString = IsDebug()
                ? secrets.GetSecret("DbConnectionString-DEV").Value.Value
                : secrets.GetSecret("DbConnectionString").Value.Value;
            string providerName = IsDebug()
                ? secrets.GetSecret("DbProviderName-DEV").Value.Value
                : secrets.GetSecret("DbProviderName").Value.Value;

            container
                .RegisterInstance<IConnectionFactory>(new DefaultConnectionFactory(connectionString, providerName))
                .RegisterInstance<IMapper>(ConfigureMapper())
                .RegisterInstance<Compiler>(DbUtil.GetCompiler(providerName))
                .RegisterSingleton<AdoManager>();

            // Managers
            container
                .RegisterType<ISkierManager, SkierManager>()
                .RegisterType<IRaceManager, RaceManager>()
                .RegisterType<ILocationManager, LocationManager>()
                .RegisterType<IStartListManager, StartListManager>()
                .RegisterType<IRaceDataManager, RaceDataManager>();

            // SignalR
            string signalREndpoint = IsDebug()
                ? ConfigurationManager.AppSettings["SignalREndpoint"].ToString()
                : secrets.GetSecret("SignalREndpoint").Value.Value;
            Console.WriteLine(signalREndpoint);
            var connection = new HubConnectionBuilder()
                .WithUrl(signalREndpoint)
                .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            container.RegisterInstance<HubConnection>(connection);

            return container;
        }
    }
}
