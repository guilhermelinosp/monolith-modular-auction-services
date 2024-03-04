using System.ComponentModel.DataAnnotations;
using System.Net;
using Auction.Authentication.Domain.DTOs.Responses;

namespace Auction.Authentication.Domain.DTOs.Abstracts;

public class BaseActionResponse<T>(HttpStatusCode? status, T? data, List<string>? error)
{
	public HttpStatusCode? Status { get; set; } = status;
	public T? Data { get; set; } = data;
	
	public List<string>? Error { get; set; } = error;
}

public class BaseActionResponse(HttpStatusCode? status, DefaultResponse? data, List<string>? error)
{
	public HttpStatusCode? Status { get; set; } = status;
	public DefaultResponse? Data { get; set; } = data;
	public List<string>? Error { get; set; } = error;
}