using System.Configuration;
using System.Data.SqlClient;
using NBlog.Web.Application.Storage;
using NBlog.Web.Application.Storage.Sql;
using NUnit.Framework;

namespace NBlog.Tests
{
    [TestFixture]
    public class SqlRepositoryTests : RepositoryTests
    {
        private static readonly string SqlConnectionString = ConfigurationManager.AppSettings["SqlConnectionString"];
        private static readonly string SqlDatabaseName = ConfigurationManager.AppSettings["SqlDatabaseName"];

        [TearDown]
        public void TestCleanup()
        {
            using (var cnn = new SqlConnection(SqlConnectionString))
            using (var cmd = new SqlCommand("EXEC sp_MSforeachtable @command1 = 'TRUNCATE TABLE ?'", cnn))
            {
                cnn.Open();
                cnn.ChangeDatabase(SqlDatabaseName);
                cmd.ExecuteNonQuery();
            }
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            const string dropSql = @"
                ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
                DROP DATABASE [{0}]";

            using (var cnn = new SqlConnection(SqlConnectionString))
            using (var cmd = new SqlCommand(string.Format(dropSql, SqlDatabaseName), cnn))
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        protected override IRepository GetRepository()
        {
            return new SqlRepository(Keys, SqlConnectionString, SqlDatabaseName);
        }
    }
}
