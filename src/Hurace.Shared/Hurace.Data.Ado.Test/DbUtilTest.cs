using System;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using SqlKata.Compilers;
using Xunit;

namespace Hurace.Data.Ado.Test
{
    public class DbUtilTest
    {
        [Fact]
        public void GetDbProvicerFactoryValidTest()
        {
            Assert.IsType<SqlClientFactory>(DbUtil.GetDbProvicerFactory("Microsoft.Data.SqlClient"));
            Assert.IsType<MySqlClientFactory>(DbUtil.GetDbProvicerFactory("MySql.Data.MySqlClient"));
        }

        [Fact]
        public void GetDbProvicerFactoryInvalidTest()
        {
            Assert.Throws<ArgumentException>(() => DbUtil.GetDbProvicerFactory("Invalid.Provider"));
        }

        [Fact]
        public void GetCompilerValidTest()
        {
            Assert.IsType<SqlServerCompiler>(DbUtil.GetCompiler("Microsoft.Data.SqlClient"));
            Assert.IsType<MySqlCompiler>(DbUtil.GetCompiler("MySql.Data.MySqlClient"));
        }

        [Fact]
        public void GetCompilerInvalidTest()
        {
            Assert.Throws<ArgumentException>(() => DbUtil.GetDbProvicerFactory("Invalid.Compiler"));
        }
    }
}
