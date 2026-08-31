namespace AuctionService.Tests;

public class AuctionEntityTests
{
    // Method_Scenario_Expected
    [Fact]
    public void HasReservePrice_WithReserveGtZero_ReturnsTrue()
    {
        // arrange
        var auction = TestData.CreateAuction();

        // act
        var result = auction.HasReservePrice();

        // assert
        Assert.True(result);
    }
    
    [Fact]
    public void HasReservePrice_WithNoReserve_ReturnsFalse()
    {
        // arrange
        var auction = TestData.CreateAuction(x => x.ReservePrice = 0);

        // act
        var result = auction.HasReservePrice();

        // assert
        Assert.False(result);
    }
}