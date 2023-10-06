using System.Reflection;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;
public class JwtAppContext : DbContext
{
    public JwtAppContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Rol> Rols { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRol> UsersRols { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    // dotnet ef database update --project ./Persistencia/ --startup-project ./API/
    // dotnet ef migrations add InitialCreate --project .\Persistencia\ --startup-project ./API/ --output-dir ./Data/Migrations
}
