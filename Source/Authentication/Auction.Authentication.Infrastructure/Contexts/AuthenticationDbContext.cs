using Auction.Authentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auction.Authentication.Infrastructure.Contexts;

public class AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : DbContext(options)
{
	public DbSet<Account>? Accounts { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthenticationDbContext).Assembly);
	}
}