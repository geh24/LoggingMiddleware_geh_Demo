using LoggingMiddleware_geh;

namespace LoggingMiddleware_geh_Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                       .AddJsonFile("appsettings.json")
                       .Build();
            LoggingMiddleware_geh.LoggingMiddleware_geh.init(config);
            LMLogger log = LoggingMiddleware_geh.LoggingMiddleware_geh.getLogger();

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

            // Setting enableTraceBody, enableTraceHeaders, enableTraceQueryString is optional
            LoggingMiddleware_geh.LoggingMiddleware_geh.enableTraceBody(true);
            LoggingMiddleware_geh.LoggingMiddleware_geh.enableTraceHeaders(true);
            LoggingMiddleware_geh.LoggingMiddleware_geh.enableTraceQueryString(true);
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
                log.Error("Stopped program because of exception", e);
                throw;
            } finally {
//                NLog.LogManager.Shutdown();
            }
        }
    }
}
