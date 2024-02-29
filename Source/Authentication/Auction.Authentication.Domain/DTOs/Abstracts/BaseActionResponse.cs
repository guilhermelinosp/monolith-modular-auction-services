using Auction.Authentication.Domain.DTOs.Responses;

namespace Auction.Authentication.Domain.DTOs.Abstracts;

public class BaseActionResponse<T>(bool success, T? data, List<string>? error)
{
	public bool Success { get; set; } = success;
	public T? Data { get; set; } = data;
	public List<string>? Error { get; set; } = error;
}

public class BaseActionResponse(bool success, DefaultResponse? data, List<string>? error)
{
	public bool Success { get; set; } = success;
	public DefaultResponse? Data { get; set; } = data;
	public List<string>? Error { get; set; } = error;
}