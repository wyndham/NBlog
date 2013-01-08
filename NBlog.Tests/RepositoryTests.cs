using System;
using System.Linq;
using NBlog.Web.Application.Infrastructure;
using NBlog.Web.Application.Service.Entity;
using NBlog.Web.Application.Storage;
using NUnit.Framework;

namespace NBlog.Tests
{
    public abstract class RepositoryTests
    {
        protected static readonly RepositoryKeys Keys;

        static RepositoryTests()
        {
            Keys = new RepositoryKeys();
            Keys.Add<Entry>(e => e.Slug);
            Keys.Add<Config>(c => c.Site);
            Keys.Add<User>(u => u.Username);
        }

        [SetUp]
        public void SetUp()
        {
            Instance = GetRepository();
        }

        public IRepository Instance;

        protected abstract IRepository GetRepository();

        [Test]
        public void Single_Should_Return_Correct_Entity_By_Key()
        {
            // arrange
            var repository = Instance;
            const string title = "Top 10 C# Tips";
            var slug = title.ToUrlSlug();
            var entry = new Entry { Slug = slug, Title = title, DateCreated = DateTime.Now };

            // act
            repository.Save(entry);

            var keyValue = Keys.GetKeyValue(entry);
            var retrievedEntry = repository.Single<Entry>(keyValue);

            // assert
            Assert.AreEqual(retrievedEntry.Title, title);
        }


        [Test, ExpectedException(typeof(Exception))]
        //[Test]
        public void Single_Should_Throw_When_Entity_Does_Not_Exist()
        {
            // arrange
            var repository = Instance;

            // act
            var entry = repository.Single<Entry>("this-entry-does-not-exist");
        }


        [Test]
        public void List_Should_Return_All_Entities()
        {
            // arrange
            var repository = Instance;
            repository.Save(new Entry { Slug = "entry-1", Title = "Entry 1", DateCreated = DateTime.Now });
            repository.Save(new Entry { Slug = "entry-2", Title = "Entry 2", DateCreated = DateTime.Now });
            repository.Save(new Entry { Slug = "entry-3", Title = "Entry 3", DateCreated = DateTime.Now });

            // act
            var all = repository.All<Entry>();

            // assert
            Assert.IsTrue(all.Count() == 3);
        }


        [Test]
        public void Exists_Should_Be_True_When_Entity_Exists()
        {
            // arrange
            var repository = Instance;
            repository.Save(new Entry { Slug = "entry-1", Title = "Entry 1", DateCreated = DateTime.Now });

            // act
            var exists = repository.Exists<Entry>("entry-1");

            // assert
            Assert.IsTrue(exists);
        }


        [Test]
        public void Exists_Should_Be_False_When_Entity_Deleted()
        {
            // arrange
            var repository = Instance;
            repository.Save(new Entry { Slug = "entry-1", Title = "Entry 1", DateCreated = DateTime.Now });
            repository.Delete<Entry>("entry-1");

            // act
            var exists = repository.Exists<Entry>("entry-1");

            // assert
            Assert.IsFalse(exists);
        }
    }
}