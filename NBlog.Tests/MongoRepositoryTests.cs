using System.Configuration;
using MongoDB.Driver;
using NBlog.Web.Application.Storage;
using NBlog.Web.Application.Storage.Mongo;
using NUnit.Framework;

namespace NBlog.Tests
{
    [TestFixture]
    public class MongoRepositoryTests : RepositoryTests
    {
        private static readonly string MongoConnectionString = ConfigurationManager.AppSettings["MongoConnectionString"];
        private static readonly string MongoDatabaseName = ConfigurationManager.AppSettings["MongoDatabaseName"];

        [TearDown]
        public void TestCleanup()
        {
            var server = MongoServer.Create(MongoConnectionString);
            server.DropDatabase(MongoDatabaseName);
        }

        protected override IRepository GetRepository()
        {
            return new MongoRepository(Keys, MongoConnectionString, MongoDatabaseName);
        }
    }
}
