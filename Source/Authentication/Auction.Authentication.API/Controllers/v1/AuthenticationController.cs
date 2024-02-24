using Microsoft.AspNetCore.Mvc;

namespace Auction.Authentication.API.Controllers.v1;

[ApiController]
[Route("api/[controller]/v{version:apiVersion}")]
[ApiVersion("1.0")]
public class AuthenticationController : Controller
{
	
}