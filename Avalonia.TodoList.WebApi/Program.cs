
using Avalonia.TodoList.WebApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Avalonia.TodoList.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            // Add DbContext to the container
            builder.Services.AddDbContext<TodoDbContext>();
            // (SAMPLE) builder.Services.AddScoped<ISuperHeroRepository, SuperHeroRepository>();

            // Add logging (this is usually already configured by default)
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();


            var app = builder.Build();
            Configure(app);     // configuration and check if any migration needed to be applied.
            app.Run();
        }

        private static void Configure(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                // Apply migrations
                ApplyMigrations(app);
            }

            // Middleware to redirect from root to Swagger UI
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    context.Response.Redirect("/swagger/index.html");
                    return;
                }

                await next();
            });

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
        }

        private static void ApplyMigrations(WebApplication app)
        {
            try
            {
                using (var scope = app.Services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();

                    // Check if there are pending migrations and apply them
                    var pendingMigrations = dbContext.Database.GetPendingMigrations();
                    if (pendingMigrations.Any())
                    {
                        dbContext.Database.Migrate();
                        Console.WriteLine("Applied pending migrations.");
                    }
                    else
                    {
                        Console.WriteLine("No pending migrations.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the migration failure
                var logger = app.Services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while applying migrations.");
            }
        }

    }
}
