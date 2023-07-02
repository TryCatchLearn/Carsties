using AuctionService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionService.IntegrationTests;

public static class ServiceCollectionExtensions
{
    public static void RemoveDbContext<T>(this IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<AuctionDbContext>));

        if (descriptor != null) services.Remove(descriptor);
    }

    public static void EnsureCreated<T>(this IServiceCollection services)
    {
        var sp = services.BuildServiceProvider();

        using var scope = sp.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<AuctionDbContext>();

        db.Database.Migrate();
        DbHelper.InitDbForTests(db);
    }
}
