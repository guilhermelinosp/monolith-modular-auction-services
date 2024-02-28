using Auction.Authentication.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity.Data;

namespace Auction.Authentication.Application.Services.AutoMapper;

public class AutoMapperProfiles : Profile
{
	public AutoMapperProfiles()
	{
		CreateMap<Account, Account>();
		CreateMap<RegisterRequest, Account>();
	}
}
