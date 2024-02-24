using Auction.Authentication.API.Services;
using Auction.Authentication.Application;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Configure services
builder.Services.AddApplication(configuration);
builder.Services.AddSwagger();
builder.Services.AddVersioning();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
	});
}
else
{
	app.UseExceptionHandler("/error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();