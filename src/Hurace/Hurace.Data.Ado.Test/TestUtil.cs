using System.Data;
using AutoMapper;
using AutoMapper.Data;
using Hurace.Core;
using Hurace.Core.Models;
using Microsoft.Extensions.Configuration;
using SqlKata.Compilers;

namespace Hurace.Data.Ado.Test
{
    internal static class TestUtil
    {
        public static IMapper GetMapper()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddDataReaderMapping();
                cfg.CreateMap<IDataRecord, Race>();
                cfg.CreateMap<IDataRecord, Skier>();
                cfg.CreateMap<IDataRecord, RaceData>();
                cfg.CreateMap<IDataRecord, Location>();
                cfg.CreateMap<IDataRecord, StartList>();
            })
            .CreateMapper();
        }

        public static IConnectionFactory GetConnectionFactory(string connectionStringConfigName = "HuraceTestDbConnection")
        {
            IConfiguration config = ConfigurationUtil.GetConfiguration();
            return DefaultConnectionFactory.FromConfiguration(config, connectionStringConfigName);
        }

        public static AdoManager GetAdoManager()
        {
            return new AdoManager(GetConnectionFactory());
        }

        public static Compiler GetCompiler()
        {
            IConnectionFactory connection = GetConnectionFactory();
            return DbUtil.GetCompiler(connection.ProviderName);
        }
    }
}
