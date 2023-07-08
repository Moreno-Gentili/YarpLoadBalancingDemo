using Demo.LoadBalancer.Model.Services;
using Yarp.ReverseProxy.Model;

namespace Demo.LoadBalancer.Models.Extensions;

public static class HttpContextExtensions
{
    public static async Task PickDestination(this HttpContext context, string? email, ILogger logger)
    {
        string? destinationId = await context.FindUserDestinationIdByEmail(email);
        if (destinationId is null)
        {
            logger.LogError("Could not find destination for user {email}", email);
            return;
        }

        context.SetDestinationId(destinationId);
    }

    public static async Task PersistDestination(this HttpContext context, string? email, ILogger logger)
    {
        if (email is null)
        {
            logger.LogError("Could not find user email for persistence", email);
            return;
        }

        string? destinationId = context.GetDestinationId();
        if (destinationId is null)
        {
            logger.LogError("DestinationId was empty on registration of user {email}", email);
            return;
        }

        await context.UpsertUserDestination(email: email, destinationId: destinationId);
        logger.LogInformation("Persisted user {email} with destination {destinationId}", email, destinationId);
    }

    public static string? FindUserEmailInFormIfNeeded(this HttpContext context) =>
        context.IsLogginIn() || context.IsRegistering() ?
        context.FindEmailInForm() :
        null;

    public static bool IsLogginIn(this HttpContext context) =>
        context.Request.Is("/Identity/Account/Login", HttpMethod.Post);

    public static bool IsRegistering(this HttpContext context) =>
        context.Request.Is("/Identity/Account/Register", HttpMethod.Post);

    public static bool HasSuccessfullyRegistered(this HttpContext context) =>
        context.Request.Is("/Identity/Account/Register", HttpMethod.Post) &&
        context.Response.IsRedirecting();

    public static async ValueTask<string?> FindUserDestinationIdByEmail(this HttpContext context, string? email)
    {
        if (email is null or "")
        {
            return null;
        }

        var userDestinationRepository = context.RequestServices.GetRequiredService<IUserDestinationRepository>();
        string? destinationId = await userDestinationRepository.FindUserDestination(email);
        return destinationId;
    }

    public static async Task UpsertUserDestination(this HttpContext context, string email, string destinationId)
    {
        var userDestinationRepository = context.RequestServices.GetRequiredService<IUserDestinationRepository>();
        await userDestinationRepository.UpsertUserDestination(email: email, destination: destinationId);
    }

    public static string? GetDestinationId(this HttpContext httpContext) =>
        httpContext.RequestServices.GetRequiredService<IDestination>().Id;

    public static void SetDestinationId(this HttpContext httpContext, string destinationId) =>
        httpContext.RequestServices.GetRequiredService<IDestination>().Id = destinationId;

    public static string? FindEmailInForm(this HttpContext context)
    {
        context.Request.EnableBuffering();
        return context.Request.Form["Input.Email"];
    }
    
}
