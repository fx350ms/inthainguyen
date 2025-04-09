using Abp.Zero.EntityFrameworkCore;
using InTN.Authorization.Roles;
using InTN.Authorization.Users;
using InTN.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace InTN.EntityFrameworkCore;

public class InTNDbContext : AbpZeroDbContext<Tenant, Role, User, InTNDbContext>
{
    /* Define a DbSet for each entity of the application */

    public InTNDbContext(DbContextOptions<InTNDbContext> options)
        : base(options)
    {
    }
}
