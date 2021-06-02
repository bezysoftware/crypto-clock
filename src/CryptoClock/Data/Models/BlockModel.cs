using System;

namespace CryptoClock.Data.Models
{
    public record BlockModel(int Height, DateTime timestamp, int transactions)
    {        
    }
}
