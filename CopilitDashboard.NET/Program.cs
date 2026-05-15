using CopilitDashboard.NET.Controllers;

namespace CopilitDashboard.NET
{
    /// <summary>
    /// Entry point for the Copilot Dashboard application.
    /// 
    /// This ASP.NET Core application provides a dashboard for monitoring GitHub Copilot usage metrics
    /// within an enterprise organization. It exposes APIs to retrieve seat information and detailed
    /// usage metrics, with background polling to keep metrics updated.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main entry point of the application.
        /// 
        /// Configures the ASP.NET Core web application with the following components:
        /// - Controllers for handling API requests
        /// - CORS policy to allow cross-origin requests from any origin
        /// - OpenAPI documentation (Swagger) in development environment
        /// - Static file serving for the front-end dashboard (index.html)
        /// - Background task to fetch Copilot metrics on startup
        /// </summary>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register MVC controllers to handle API requests
            builder.Services.AddControllers();

            // Configure CORS (Cross-Origin Resource Sharing) policy to allow the front-end dashboard
            // to make requests from any origin. This is configured as "AllowAll" to support
            // deployment scenarios where the front-end may be served from different domains.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // Register OpenAPI (Swagger) service for API documentation
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline based on the environment
            if (app.Environment.IsDevelopment())
            {
                // Enable Swagger UI in development environment for API exploration
                app.MapOpenApi();
            }

            // Apply the CORS policy defined above to all requests
            app.UseCors("AllowAll");

            // Enable authorization middleware
            app.UseAuthorization();

            // Map controller routes to handle incoming HTTP requests
            app.MapControllers();

            // Serve default files (index.html) and static files from wwwroot directory
            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Initialize metrics in the background after the application starts.
            // The 5-second delay allows the web server to fully initialize before making
            // API calls to GitHub. This prevents connection errors during startup and ensures
            // metrics are pre-loaded without blocking the initial HTTP request.
            // The metrics are saved to disk for persistence across application restarts.
            Task.Delay(5000).ContinueWith(sa =>
            {
                CopilotController cs = new CopilotController();
                cs.GetCopilotMetricsAsync().Wait();
            });

            app.Run();
        }
    }
}
