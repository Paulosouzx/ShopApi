using Microsoft.EntityFrameworkCore;
using ShopsApi.Models;

namespace ShopsApi.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        //vai fazer tabelas com base nos modelos
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
    }
}