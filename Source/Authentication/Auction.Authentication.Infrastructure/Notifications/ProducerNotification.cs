using System.Text;
using Auction.Authentication.Domain.Models;
using Auction.Authentication.Domain.Notifications;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Serilog;

namespace Auction.Authentication.Infrastructure.Notifications;

public class ProducerNotification : IProducerNotification
{
	private static IConfiguration _configuration;
	private readonly IModel _channel;

	private readonly Lazy<IConnection> _lazyConnection = new(() =>
	{
		var factory = new ConnectionFactory
		{
			HostName = _configuration["RabbitMq:Host"],
			Port = int.Parse(_configuration["RabbitMq:Port"]!),
			UserName = _configuration["RabbitMq:Username"],
			Password = _configuration["RabbitMq:Password"]
		};

		return factory.CreateConnection();
	});

	public ProducerNotification(IConfiguration? configuration)
	{
		_configuration = configuration;
		_channel = _lazyConnection.Value.CreateModel();
		_channel.QueueDeclare(_configuration["RabbitMQ:QueueName"]!,
			false,
			false,
			false,
			null);
	}

	public void SendMessageAsync(NotificaitonModel message)
	{
		try
		{
			var json = JsonConvert.SerializeObject(message);
			var body = Encoding.UTF8.GetBytes(json);
			_channel.BasicPublish("",
				_configuration["RabbitMQ:QueueName"]!,
				null,
				body);
		}
		catch
		{
			Log.Error("Failed to send message to RabbitMQ");
		}
	}
}