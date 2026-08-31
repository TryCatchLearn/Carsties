using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

public class DbInitializer
{
    public static void InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        SeedData(scope.ServiceProvider.GetRequiredService<AuctionDbContext>(), app);
    }

    private static void SeedData(AuctionDbContext context, WebApplication app)
    {
        context.Database.Migrate();

        if (context.Auctions.Any() || app.Environment.IsEnvironment("Test"))
        {
            Console.WriteLine("Database Already Exists");
            return;
        }

        var auctions = new List<Auction>()
        {
            // 1 Ford GT
            new()
            {
                Id = "ford-gt",
                Status = Status.Live,
                ReservePrice = 20000,
                Seller = "bob",
                AuctionEnd = DateTime.UtcNow.AddDays(10),
                Item = new Item
                {
                    Make = "Ford",
                    Model = "GT",
                    Color = "White",
                    Mileage = 50000,
                    Year = 2020,
                    ImageUrl = "https://cdn.pixabay.com/photo/2016/05/06/16/32/car-1376190_960_720.jpg",
                    Description =
                        "A modern reincarnation of Ford's legendary Le Mans racer, this GT pairs a twin-turbo V6 with a carbon-fiber body for supercar performance. Meticulously maintained, it turns heads wherever it goes."
                }
            },
            // 2 Bugatti Veyron
            new()
            {
                Id = "bugatti-veyron",
                Status = Status.Live,
                ReservePrice = 90000,
                Seller = "alice",
                AuctionEnd = DateTime.UtcNow.AddDays(60),
                Item = new Item
                {
                    Make = "Bugatti",
                    Model = "Veyron",
                    Color = "Black",
                    Mileage = 15035,
                    Year = 2018,
                    ImageUrl = "https://cdn.pixabay.com/photo/2012/05/29/00/43/car-49278_960_720.jpg",
                    Description =
                        "An engineering marvel with a quad-turbo W16 engine delivering blistering acceleration and once-impossible top speeds. Garage-kept and lightly driven, this Veyron is a rare chance to own a true automotive icon."
                }
            },
            // 3 Ford mustang
            new()
            {
                Id = "ford-mustang",
                Status = Status.Live,
                Seller = "bob",
                AuctionEnd = DateTime.UtcNow.AddDays(4),
                Item = new Item
                {
                    Make = "Ford",
                    Model = "Mustang",
                    Color = "Black",
                    Mileage = 65125,
                    Year = 2023,
                    ImageUrl = "https://cdn.pixabay.com/photo/2012/11/02/13/02/car-63930_960_720.jpg",
                    Description =
                        "A modern Mustang with the unmistakable growl of a naturally aspirated V8. Well cared for and ready to deliver the raw, engaging driving experience Mustang fans expect."
                }
            },
            // 4 Mercedes SLK
            new()
            {
                Id = "mercedes-slk",
                Status = Status.ReserveNotMet,
                ReservePrice = 50000,
                Seller = "tom",
                AuctionEnd = DateTime.UtcNow.AddDays(-10),
                Item = new Item
                {
                    Make = "Mercedes",
                    Model = "SLK",
                    Color = "Silver",
                    Mileage = 15001,
                    Year = 2020,
                    ImageUrl = "https://cdn.pixabay.com/photo/2016/04/17/22/10/mercedes-benz-1335674_960_720.png",
                    Description =
                        "A sleek SLK roadster that blends German engineering with timeless open-top style. The retractable hardtop and smooth engine make it an effortless weekend cruiser with low mileage for its age."
                }
            },
            // 5 BMW X1
            new()
            {
                Id = "bmw-x1",
                Status = Status.Live,
                ReservePrice = 20000,
                Seller = "alice",
                AuctionEnd = DateTime.UtcNow.AddDays(30),
                Item = new Item
                {
                    Make = "BMW",
                    Model = "X1",
                    Color = "White",
                    Mileage = 90000,
                    Year = 2017,
                    ImageUrl = "https://cdn.pixabay.com/photo/2017/08/31/05/47/bmw-2699538_960_720.jpg",
                    Description =
                        "A practical BMW compact SUV that balances everyday comfort with the brand's signature driving feel. Higher mileage but well maintained, making it a solid choice for a dependable daily driver."
                }
            },
            // 6 Ferrari spider
            new()
            {
                Id = "ferrari-spider",
                Status = Status.Live,
                ReservePrice = 20000,
                Seller = "bob",
                AuctionEnd = DateTime.UtcNow.AddDays(45),
                Item = new Item
                {
                    Make = "Ferrari",
                    Model = "Spider",
                    Color = "Red",
                    Mileage = 50000,
                    Year = 2015,
                    ImageUrl = "https://cdn.pixabay.com/photo/2017/11/09/01/49/ferrari-458-spider-2932191_960_720.jpg",
                    Description =
                        "A drop-top Ferrari spider that delivers open-air thrills alongside a screaming naturally aspirated V8. A head-turning weekend car with the kind of exhaust note that never gets old."
                }
            },
            // 7 Ferrari F-430
            new()
            {
                Id = "ferrari-f430",
                Status = Status.Live,
                ReservePrice = 150000,
                Seller = "alice",
                AuctionEnd = DateTime.UtcNow.AddDays(13),
                Item = new Item
                {
                    Make = "Ferrari",
                    Model = "F-430",
                    Color = "Red",
                    Mileage = 5000,
                    Year = 2022,
                    ImageUrl = "https://cdn.pixabay.com/photo/2017/11/08/14/39/ferrari-f430-2930661_960_720.jpg",
                    Description =
                        "A pristine, low-mileage F-430 with a high-revving V8 and razor-sharp handling. Barely broken in, this is as close to a showroom Ferrari as you'll find at auction."
                }
            },
            // 8 Audi R8
            new()
            {
                Id = "audi-r8",
                Status = Status.Live,
                Seller = "bob",
                AuctionEnd = DateTime.UtcNow.AddDays(19),
                Item = new Item
                {
                    Make = "Audi",
                    Model = "R8",
                    Color = "White",
                    Mileage = 10050,
                    Year = 2021,
                    ImageUrl = "https://cdn.pixabay.com/photo/2019/12/26/20/50/audi-r8-4721217_960_720.jpg",
                    Description =
                        "A mid-engine Audi supercar that combines everyday usability with genuine performance credentials. Low mileage and a clean history make this R8 a rare find at this price point."
                }
            },
            // 9 Audi TT
            new()
            {
                Id = "audi-tt",
                Status = Status.Live,
                ReservePrice = 20000,
                Seller = "tom",
                AuctionEnd = DateTime.UtcNow.AddDays(20),
                Item = new Item
                {
                    Make = "Audi",
                    Model = "TT",
                    Color = "Black",
                    Mileage = 25400,
                    Year = 2020,
                    ImageUrl = "https://cdn.pixabay.com/photo/2016/09/01/15/06/audi-1636320_960_720.jpg",
                    Description =
                        "A sharp, compact Audi coupe with distinctive styling and confident handling. Well within its service intervals, it offers a fun and stylish alternative to more common sports coupes."
                }
            },
            // 10 Ford Model T
            new()
            {
                Id = "ford-model-t",
                Status = Status.Live,
                ReservePrice = 20000,
                Seller = "bob",
                AuctionEnd = DateTime.UtcNow.AddDays(48),
                Item = new Item
                {
                    Make = "Ford",
                    Model = "Model T",
                    Color = "Rust",
                    Mileage = 150150,
                    Year = 1938,
                    ImageUrl = "https://cdn.pixabay.com/photo/2017/08/02/19/47/vintage-2573090_960_720.jpg",
                    Description =
                        "A genuine piece of automotive history, this Model T has survived decades and racked up plenty of miles along the way. A patinated project car for the collector who values character over polish."
                }
            }
        };

        context.Auctions.AddRange(auctions);
        
        context.SaveChanges();
    }
}