using Auction.Authentication.Domain.Entities;

namespace Auction.Authentication.Domain.Repositories;

public interface IAccountRepository
{
	Task<Account?> FindByIdAsync(string accountId);
	Task<Account?> FindByEmailAsync(string email);
	Task CreateAsync(Account account);
	Task UpdateAsync(Account account);
}