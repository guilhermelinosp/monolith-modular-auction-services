namespace Auction.Authentication.Domain.DTOs.Abstracts;

public class BaseActionResponse<T>(bool success,T? data, List<string>? error )
{
	public bool Success { get; set; } = success;
	public T? Data { get; set; } = data;
	public List<string>? Error { get; set; } = error;
}