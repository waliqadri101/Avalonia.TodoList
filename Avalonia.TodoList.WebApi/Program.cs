
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

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Middleware to redirect from root to Swagger UI
            app.Use(async (context, next) =>
            {
                // Check if the request is for the root URL
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
            app.Run();
        }
    }
}
