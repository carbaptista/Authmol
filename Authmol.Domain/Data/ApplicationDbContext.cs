using Authmol.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authmol.Domain.Data;
public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Endereco> Enderecos { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}
