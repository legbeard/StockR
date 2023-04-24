using Orleans;

namespace Stockr.API.Model;

[GenerateSerializer]
public record Stock(String Symbol, double Bid, double Ask)
{
    public Stock() : this(null, 0, 0)
    {
        
    }
};