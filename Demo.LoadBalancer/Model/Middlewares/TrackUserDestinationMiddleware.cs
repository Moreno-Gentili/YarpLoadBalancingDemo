using Demo.LoadBalancer.Models.Extensions;

namespace Demo.LoadBalancer.Model.Middlewares;

public class TrackUserDestinationMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<TrackUserDestinationMiddleware> logger;

    public TrackUserDestinationMiddleware(RequestDelegate next, ILogger<TrackUserDestinationMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string? email = context.FindUserEmailInFormIfNeeded();
        
        if (context.IsLogginIn())
        {
            await context.PickDestination(email, logger);
        }

        await next(context);

        if (context.HasSuccessfullyRegistered())
        {
            await context.PersistDestination(email, logger);
        }
    }
}