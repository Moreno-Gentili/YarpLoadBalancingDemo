using Demo.LoadBalancer.Model.LoadBalancingPolicies;
using Demo.LoadBalancer.Model.Middlewares;
using Demo.LoadBalancer.Model.Services;
using Microsoft.AspNetCore.DataProtection;
using Yarp.ReverseProxy.LoadBalancing;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\Progetti\LoadBalancing\Keys"))
    .SetApplicationName("SharedCookieApp");

builder.Services.AddAuthentication("Identity.Application")
    .AddCookie("Identity.Application", options =>
    {
        options.Cookie.Name = ".AspNet.SharedCookie";
    });

builder.Services.AddSingleton<ILoadBalancingPolicy, UserLoadBalancingPolicy>();
builder.Services.AddSingleton<IUserDestinationRepository, UserDestinationRepository>();
builder.Services.AddScoped<IDestination, Destination>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
app.MapReverseProxy(proxyPipeline =>
{
    proxyPipeline.UseMiddleware<TrackUserDestinationMiddleware>();
    // proxyPipeline.UseSessionAffinity();
    proxyPipeline.UseLoadBalancing();
    proxyPipeline.UsePassiveHealthChecks();
});
app.Run();
