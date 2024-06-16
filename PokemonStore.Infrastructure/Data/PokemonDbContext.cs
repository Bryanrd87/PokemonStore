using Microsoft.EntityFrameworkCore;
using PokemonStore.Domain.Entities;

namespace PokemonStore.Infrastructure.Data;
public class PokemonDbContext : DbContext
{
    public DbSet<Pokemon> Pokemons { get; set; }

    public PokemonDbContext(DbContextOptions<PokemonDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pokemon>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
        });
    }
}
