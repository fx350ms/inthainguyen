using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace InTN.EntityFrameworkCore;

public static class InTNDbContextConfigurer
{
    public static void Configure(DbContextOptionsBuilder<InTNDbContext> builder, string connectionString)
    {
        builder.UseSqlServer(connectionString);
    }

    public static void Configure(DbContextOptionsBuilder<InTNDbContext> builder, DbConnection connection)
    {
        builder.UseSqlServer(connection);
    }
}
