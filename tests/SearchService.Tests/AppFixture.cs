using Alba;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Wolverine;

namespace SearchService.Tests;

public class AppFixture : IAsyncLifetime
{
    private readonly IContainer _meilisearchContainer 
        = new ContainerBuilder("getmeili/meilisearch:v1.53.1")
            .WithPortBinding(7700, true)
            .WithEnvironment("MEILI_MASTER_KEY", "masterkey")
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilHttpRequestIsSucceeded(request => 
                    request.ForPath("/health").ForPort(7700)))
            .Build();

    public IAlbaHost Host { get; private set; }
    
    public async Task InitializeAsync()
    {
        await _meilisearchContainer.StartAsync();
        
        var meilisearchUrl = 
            $"http://{_meilisearchContainer.Hostname}:{_meilisearchContainer.GetMappedPublicPort(7700)}";

        Host = await AlbaHost.For<Program>(builder =>
        {
            builder.UseEnvironment("Test");
            builder.UseSetting("Meilisearch:Url", meilisearchUrl);
            builder.UseSetting("Meilisearch:ApiKey", "masterkey");
            builder.ConfigureServices(services =>
            {
                services.RunWolverineInSoloMode();
                services.DisableAllExternalWolverineTransports();
            });
        });
    }

    public async Task DisposeAsync()
    {
        await Host.StopAsync();
        await Host.DisposeAsync();
        await _meilisearchContainer.DisposeAsync();
    }
}

[CollectionDefinition("search-service")]
public class SearchServiceCollection : ICollectionFixture<AppFixture>;