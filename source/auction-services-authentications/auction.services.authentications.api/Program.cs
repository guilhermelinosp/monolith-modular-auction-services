using auction.services.authentications.api.Filters;
using auction.services.authentications.api.Services;
using auction.services.authentications.application;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

await builder.Services.AddApplicationInjection(configuration);
builder.Services.AddSwaggerServ();
builder.Services.AddCorsServ();
builder.Services.AddVersioningServ();
builder.Services.AddRoutingServ();
builder.Services.AddAuthenticationServ(configuration);
builder.Services.AddControllers(options => { options.Filters.AddService<ExceptionFilter>(); });
builder.Services.AddScoped<ExceptionFilter>();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwaggerDoc();
	app.UseDeveloperExceptionPage();
	configuration.AddUserSecrets<Program>();
}
else
{
	app.UseExceptionHandler("/error");
	app.UseHsts();
}

app.UseCors(builder =>
{
	builder
		.AllowAnyOrigin()
		.AllowAnyMethod()
		.AllowAnyHeader();
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseApiVersioning();
app.UseAuthorization();
app.MapControllers();

app.Run();