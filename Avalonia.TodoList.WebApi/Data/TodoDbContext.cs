using Avalonia.TodoList.WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Avalonia.TodoList.WebApi.Data
{
    public class TodoDbContext : DbContext
    {
        private readonly IConfiguration? _configuration;

        public DbSet<Todo> Todos { get; set; }

        #region CONSTRUCTORS
        public TodoDbContext(DbContextOptions<TodoDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }
        public TodoDbContext() : base() { }
        #endregion CONSTRUCTORS

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// If you are not configuring your Database connection in the DI in program.cs, then you have to do it here.
        /// I prefer to do it here for debugging purposes.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Create a configuration builder
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                // Get the connection string from the configuration
                var connectionString = config.GetConnectionString("DefaultConnection");

                // Configure the context to use SQL Server
                optionsBuilder.UseSqlServer(connectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
}