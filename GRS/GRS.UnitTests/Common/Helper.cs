using System;
using System.Diagnostics;
using System.Linq;
using GRS.Web.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GRS.UnitTests.Common
{
    public static class Helper
    {
        public static Func<ApplicationDbContext> InMemoryContextCreator()
        {
            var databaseName = Guid.NewGuid().ToString();
            Debug.WriteLine($"databaseName {databaseName}");

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                            .Options;

            return () => new ApplicationDbContext(options);
        }

        public static DbSet<T> GetQueryableMockDbSet<T>(params T[] sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return dbSet.Object;
        }
    }
}
