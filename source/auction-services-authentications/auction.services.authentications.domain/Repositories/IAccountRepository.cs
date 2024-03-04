using auction.services.authentications.domain.Entities;

namespace auction.services.authentications.domain.Repositories;

public interface IAccountRepository
{
	Task<Account?> FindByIdAsync(string accountId);
	Task<Account?> FindByEmailAsync(string email);
	Task CreateAsync(Account account);
	Task UpdateAsync(Account account);
}