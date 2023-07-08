namespace Demo.LoadBalancer.Models.Extensions;

public static class HttpRequestExtensions
{
    public static bool Is(this HttpRequest request, string path, HttpMethod method) =>
        path.Equals(request.Path, StringComparison.CurrentCultureIgnoreCase) &&
        method.ToString().Equals(request.Method, StringComparison.CurrentCultureIgnoreCase);
}
