namespace Demo.LoadBalancer.Model.Services;

public class Destination : IDestination
{
    private const string Key = "DestinationId";
    private string? id;
    private readonly HttpContext context;

    public Destination(IHttpContextAccessor httpContextAccessor)
    {
        this.context = httpContextAccessor.HttpContext ??
                       throw new InvalidOperationException("HttpContext not available");
    }

    public string? Id
    {
        get
        {
            return id ??
                   context.User.FindFirst(Key)?.Value;
        }

        set
        {
            id = value;
        }
    }
}