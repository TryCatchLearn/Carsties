using NotificationService.Hubs;
using Wolverine;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Host.UseWolverine(opts =>
{
    opts.UseRabbitMq(rabbit =>
        {
            rabbit.HostName = builder.Configuration["RabbitMQ:Host"] ?? "localhost";
            rabbit.UserName = builder.Configuration["RabbitMQ:Username"] ?? "guest";
            rabbit.Password = builder.Configuration["RabbitMQ:Password"] ?? "guest";
        })
        .DeclareExchange("auction-created", ex => ex.ExchangeType = ExchangeType.Fanout)
        .DeclareExchange("auction-finished", ex => ex.ExchangeType = ExchangeType.Fanout)
        .DeclareExchange("bid-placed", ex => ex.ExchangeType = ExchangeType.Fanout)
        .BindExchange("auction-created").ToQueue("notification-auction-created")
        .BindExchange("auction-finished").ToQueue("notification-auction-finished")
        .BindExchange("bid-placed").ToQueue("notification-bid-placed")
        .AutoProvision();
    
    opts.ListenToRabbitQueue("notification-auction-created");
    opts.ListenToRabbitQueue("notification-auction-finished");
    opts.ListenToRabbitQueue("notification-bid-placed");
});

var app = builder.Build();

app.MapHub<NotificationHub>("/notifications");

app.Run();