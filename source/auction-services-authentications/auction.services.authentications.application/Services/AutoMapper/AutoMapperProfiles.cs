using auction.services.authentications.domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity.Data;

namespace auction.services.authentications.application.Services.AutoMapper;

public class AutoMapperProfiles : Profile
{
	public AutoMapperProfiles()
	{
		CreateMap<Account, Account>();
		CreateMap<RegisterRequest, Account>();
	}
}