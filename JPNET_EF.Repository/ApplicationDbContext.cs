namespace JPNET_EF.Repository;

using Abstractions.Entity;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Client> Clients { get; set; }
    public DbSet<InternetClient> InternetClients { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderedItem> OrderedItems { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>()
            .HasDiscriminator<char>("clientType")
            .HasValue<Client>('N')
            .HasValue<InternetClient>('I');
        modelBuilder.Entity<Order>()
            .HasDiscriminator<char>("orderType")
                .HasValue<Order>('N')
                .HasValue<InternetOrder>('I');

    }
}