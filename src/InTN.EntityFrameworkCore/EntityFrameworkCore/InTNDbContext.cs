using Abp.Zero.EntityFrameworkCore;
using InTN.Authorization.Roles;
using InTN.Authorization.Users;
using InTN.Entities;
using InTN.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace InTN.EntityFrameworkCore;

public class InTNDbContext : AbpZeroDbContext<Tenant, Role, User, InTNDbContext>
{
    /* Define a DbSet for each entity of the application */

    public DbSet<Process> Processes { get; set; }
    public DbSet<ProcessStep> ProcessSteps { get; set; }
    public DbSet<ProcessStepGroup> ProcessStepGroups { get; set; }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<IdentityCode> IdentityCodes { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<OrderAttachment> OrderAttachments { get; set; } 
    public DbSet<OrderLog> OrderLogs { get; set; }

    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<CustomerBalanceHistory> CustomerBalanceHistories { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<ProductProperty> ProductProperties { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<FileUpload> FileUploads { get; set; }
    public DbSet<ProductPriceCombination> ProductPriceCombinations { get; set; }

    public DbSet<ProductNote> ProductNotes { get; set; }
    public DbSet<Printer> Printers { get; set; }

    public InTNDbContext(DbContextOptions<InTNDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
