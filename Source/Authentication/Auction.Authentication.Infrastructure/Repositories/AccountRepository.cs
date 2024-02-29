using Auction.Authentication.Domain.Entities;
using Auction.Authentication.Domain.Repositories;
using Auction.Authentication.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Auction.Authentication.Infrastructure.Repositories;

public class AccountRepository(AuthenticationDbContext context) : IAccountRepository
{
	public async Task<Account?> FindByIdAsync(string accountId)
	{
		try
		{
			return await context.Accounts!.AsNoTracking().FirstOrDefaultAsync(a => a.Id == new Guid(accountId));
		}
		catch (Exception e)
		{
			Log.Error(e, "An error occurred while executing FindByIdAsync");
			throw;
		}
	}

	public async Task<Account?> FindByEmailAsync(string email)
	{
		try
		{
			return await context.Accounts!.AsNoTracking().FirstOrDefaultAsync(a => a.Email == email);
		}
		catch (Exception e)
		{
			Log.Error(e, "An error occurred while executing FindByEmailAsync");
			throw;
		}
	}

	public async Task CreateAsync(Account account)
	{
		try
		{
			await context.Accounts!.AddAsync(account);
			await SaveChangesAsync();
		}
		catch (Exception e)
		{
			Log.Error(e, "An error occurred while executing CreateAsync");
			throw;
		}
	}

	public async Task UpdateAsync(Account account)
	{
		try
		{
			context.Accounts!.Update(account);
			await SaveChangesAsync();
		}
		catch (Exception e)
		{
			Log.Error(e, "An error occurred while executing UpdateAsync");
			throw;
		}
	}

	private async Task SaveChangesAsync()
	{
		try
		{
			await context.SaveChangesAsync();
		}
		catch (Exception e)
		{
			Log.Error(e, "An error occurred while executing SaveChangesAsync");
			throw;
		}
	}
}