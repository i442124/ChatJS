using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ChatJS.Data.Tests
{
    public class DbContextOptionsProvider
    {
        public static DbContextOptions<ApplicationDbContext> InMemory
        {
            get { return CreateContextOptions(); }
        }

        private static DbContextOptions<ApplicationDbContext> CreateContextOptions()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase("ChatJS", new InMemoryDatabaseRoot());
            return builder.Options;
        }
    }
}
