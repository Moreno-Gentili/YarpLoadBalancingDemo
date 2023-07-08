using Demo.LoadBalancer.Models.Extensions;
using Yarp.ReverseProxy.LoadBalancing;
using Yarp.ReverseProxy.Model;

namespace Demo.LoadBalancer.Model.LoadBalancingPolicies;

public sealed class UserLoadBalancingPolicy : ILoadBalancingPolicy
{
    public string Name => "User";

    public DestinationState? PickDestination(HttpContext context, ClusterState cluster, IReadOnlyList<DestinationState> availableDestinations)
    {
        string? destinationId = context.GetDestinationId();
        
        if (destinationId is null)
        {
            DestinationState destination = availableDestinations.PickAtRandom();
            context.SetDestinationId(destination.DestinationId);
            return destination;
        }
        else
        {
            return availableDestinations.PickById(destinationId);
        }
    }
}
