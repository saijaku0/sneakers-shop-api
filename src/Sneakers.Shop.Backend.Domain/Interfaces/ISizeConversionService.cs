
using Sneakers.Shop.Backend.Domain.Enums;

namespace Sneakers.Shop.Backend.Domain.Interfaces
{
    public interface ISizeConversionService
    {
        decimal GetEquivalentSize(decimal size, MeasureSizes measureType, MeasureSizes targetMeasureType, string audience);
    }
}
