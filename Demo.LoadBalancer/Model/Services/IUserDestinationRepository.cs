namespace Demo.LoadBalancer.Model.Services;

public interface IUserDestinationRepository
{
    Task<string?> FindUserDestination(string email);
    Task UpsertUserDestination(string email, string destination);
}