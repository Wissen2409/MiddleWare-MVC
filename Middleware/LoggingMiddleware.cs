using System.Diagnostics;
public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    //private  MongoDbClient _mongoDbClient;
    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
        //_mongoDbClient = mongoDbClient;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        string body = "";
        // log atacaz: 
        // loglama middleware
        if (context.Request != null)
        {
            if (context.Request.Body != null)
            {
                // Request'in body'si direk olarak erişebilir değil
                // Body byte cinsinden duran bir tip, byte cinsinden veriyi stream ile okumak mümkün
                // Body'in üstüne mouse ile gelirseniz, stream döndüğünü görebilirsiniz

                // bizde StreamReader ile body'i okuduk
                // byte olduğu için, son satıra kadar okuma yapmak için, ReadToEnd dedik!!

                var reader = new StreamReader(context.Request.Body);
                body = reader.ReadToEndAsync().Result;

            }
        }


        _next(context).Wait();
        LogModel model = new LogModel()
        {
            Date = DateTime.Now.ToLongTimeString(),
            Path = context.Request.Path,
            Body = body
        };
        new MongoDbClient().AddLog(model);
        stopwatch.Stop();
    }

}