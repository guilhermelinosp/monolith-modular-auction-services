using auction.services.authentications.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace auction.services.authentications.infrastructure.Contexts;

public class AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : DbContext(options)
{
	public DbSet<Account>? Accounts { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthenticationDbContext).Assembly);
	}
}