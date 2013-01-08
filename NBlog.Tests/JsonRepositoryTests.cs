using System.IO;
using NBlog.Web.Application.Infrastructure;
using NBlog.Web.Application.Storage;
using NBlog.Web.Application.Storage.Json;
using NUnit.Framework;

namespace NBlog.Tests
{
    [TestFixture]
    public class JsonRepositoryTests : RepositoryTests
    {
        private static readonly string JsonWorkingFolder;

        static JsonRepositoryTests()
        {
            JsonWorkingFolder = Path.Combine(Path.GetTempPath(), "NBlogIntegrationTests");
        }

        [TearDown]
        public void TestCleanup()
        {
            if (Directory.Exists(JsonWorkingFolder))
            {
                Directory.Delete(JsonWorkingFolder, recursive: true);
            }
        }

        protected override IRepository GetRepository()
        {
            return new JsonRepository(Keys, new HttpTenantSelector());
        }
    }
}
