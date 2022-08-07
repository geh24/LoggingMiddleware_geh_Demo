using LoggingMiddleware;
using NLog;

namespace LoggingMiddleware_geh_Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LoggingMiddleware.LoggingMiddleware.init("nlog.config");
            Logger log = LoggingMiddleware.LoggingMiddleware.getLoger();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            LoggingMiddleware.LoggingMiddleware.enableTrace(true);
            app.UseLoggingMiddleware();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            try {
                log.Info("init main");
                app.Run();
                log.Info("shutdown main");
            } catch (Exception e) {
                //NLog: catch setup errors
                log.Error(e, "Stopped program because of exception");
                throw;
            } finally {
                NLog.LogManager.Shutdown();
            }
        }
    }
}
