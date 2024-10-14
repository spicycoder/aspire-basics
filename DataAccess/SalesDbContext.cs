using DataAccess.Entities;

using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class SalesDbContext(DbContextOptions<SalesDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
}