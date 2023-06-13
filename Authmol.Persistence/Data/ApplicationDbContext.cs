using Authmol.Persistence.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
#nullable disable

namespace Authmol.Persistence.Data;
public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Endereco> Enderecos { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}
