namespace Demo.LoadBalancer.Models.Extensions;

public static class HttpResponseExtensions
{
    public static bool IsRedirecting(this HttpResponse response) =>
        response.StatusCode == 302;
}
