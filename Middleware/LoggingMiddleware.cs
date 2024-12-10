using System.Diagnostics;
public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    //private  MongoDbClient _mongoDbClient;
    public LoggingMiddleware(RequestDelegate next )
    {
        _next = next;
        //_mongoDbClient = mongoDbClient;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        // log atacaz: 
        // loglama middleware

        _next(context).Wait();
        LogModel model = new LogModel()
        {
            Date = DateTime.Now.ToLongTimeString(),
            Path = context.Request.Path,
        };
        new MongoDbClient().AddLog(model);
        stopwatch.Stop();
    }

}