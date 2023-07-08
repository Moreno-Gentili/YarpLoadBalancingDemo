using Yarp.ReverseProxy.Model;

namespace Demo.LoadBalancer.Models.Extensions;

public static class DestinationStatesExtensions
{
    public static DestinationState PickById(this IReadOnlyCollection<DestinationState> destinations, string destinationId) =>
         destinations.Single(destination => destinationId.Equals(destination.DestinationId, StringComparison.CurrentCultureIgnoreCase));

    public static DestinationState PickLessBusy(this IReadOnlyCollection<DestinationState> destinations) =>
         destinations.OrderBy(destination => destination.ConcurrentRequestCount).First();

    public static DestinationState PickAtRandom(this IReadOnlyCollection<DestinationState> destinations) =>
         destinations.OrderBy(destination => Random.Shared.Next()).First();
}
