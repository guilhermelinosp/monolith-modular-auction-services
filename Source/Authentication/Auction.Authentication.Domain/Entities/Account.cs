using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Auction.Authentication.Domain.Enums;

namespace Auction.Authentication.Domain.Entities;

[Table("TB_ACCOUNT")]
public class Account
{
	[Key] public Guid Id { get; set; } = Guid.NewGuid();
	[Required] public required string Email { get; set; }
	[Required] public required string Password { get; set; }
	public AccountRole Role { get; set; } = AccountRole.Default;
	public bool Is2Fa { get; set; } = false;
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
	public bool IsDeleted { get; set; } = false;
	public DateTime? DeletedAt { get; set; } = null;
}