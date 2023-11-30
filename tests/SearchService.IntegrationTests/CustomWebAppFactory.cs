using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Mongo2Go;
using MongoDB.Driver;
using MongoDB.Entities;

namespace SearchService.IntegrationTests;

public class CustomWebAppFactory : WebApplicationFactory<Program>
{
    private readonly MongoDbRunner _runner;
    
    public CustomWebAppFactory()
    {
        _runner = MongoDbRunner.Start();
        DB.InitAsync("test-db", MongoClientSettings.FromConnectionString(_runner.ConnectionString));
        
        DB.Index<Item>()
            .Key(x => x.Make, KeyType.Text)
            .Key(x => x.Model, KeyType.Text)
            .Key(x => x.Color, KeyType.Text)
            .CreateAsync();
        
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureTestServices(services =>
        {
            services.AddMassTransitTestHarness();
        });
    }

    protected override void Dispose(bool disposing)
    {
        _runner.Dispose();
    }
}