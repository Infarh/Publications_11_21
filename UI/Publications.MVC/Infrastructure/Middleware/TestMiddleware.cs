namespace Publications.MVC.Infrastructure.Middleware;

public class TestMiddleware
{
    private readonly RequestDelegate _Next;

    public TestMiddleware(RequestDelegate next) => _Next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        // Мы начинаем анализ данных внутри context

        var next = _Next(context); // Вызываем следующую часть конвейера

        // параллельно конвейеру выполняем какие-то действия

        await next; // синхронизируемся с остальной цепочкой обработки запроса

        // выполняем анализ и постобработку данных в context
    }
}