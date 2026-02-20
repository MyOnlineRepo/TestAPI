using MediatR;
using Shared.Application;
using Shared.Application.AccountSettings.Abstractions;
using Shared.Infrastructure.AccountSettings;

namespace TestAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers();
			builder.Services.AddOpenApi();
			builder.Services.AddMediatR(configuration =>
			{
				configuration.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);
			});
			builder.Services.AddSingleton<IAccountSettingsRepository, InMemoryAccountSettingsRepository>();

			var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
			}

			app.UseHttpsRedirection();
			app.UseAuthorization();
			app.MapControllers();
			app.Run();
		}
	}
}
