using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Demo.Web.Model.Authentication;

public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<IdentityUser>
{
    private readonly IConfiguration configuration;

    public CustomClaimsPrincipalFactory(IConfiguration configuration, UserManager<IdentityUser> userManager, IOptions<IdentityOptions> options)
        : base(userManager, options)
    {
        this.configuration = configuration;
    }

    public override async Task<ClaimsPrincipal> CreateAsync(IdentityUser user)
    {
        ClaimsPrincipal principal = await base.CreateAsync(user);
        ClaimsIdentity identity = (principal.Identity as ClaimsIdentity) ?? throw new InvalidOperationException("Identity is not a ClaimsIdentity");
        string destinationId = configuration["DestinationId"] ?? string.Empty;
        identity.AddClaim(new Claim("DestinationId", destinationId));
        return principal;
    }
}