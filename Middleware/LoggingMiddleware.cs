using System.Diagnostics;
public class LoggingMiddleware
{

    private readonly RequestDelegate _next;
    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
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
        MongoDbClient client = new MongoDbClient();
        client.AddLog(model);
        stopwatch.Stop();
    }

}