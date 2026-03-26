using CRM.API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CRM.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<User> Users => Set<User>();

    }
}
