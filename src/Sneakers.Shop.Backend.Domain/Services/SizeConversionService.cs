using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Exceptions;
using Sneakers.Shop.Backend.Domain.Interfaces;

namespace Sneakers.Shop.Backend.Domain.Services
{
    public class SizeConversionService : ISizeConversionService
    {
        private static IReadOnlyDictionary<Audience, Dictionary<decimal, (decimal UK, decimal EU, decimal CM)>> Sizes { get => _sizes.AsReadOnly(); }

        static private readonly Dictionary<Audience, Dictionary<decimal, (decimal UK, decimal EU, decimal CM)>> _sizes = new()
        {
            [Audience.Male] = new()
                    {
                { 6.0m,  (5.5m, 39.0m, 24.0m) },
                { 7.0m,  (6.5m, 40.0m, 25.0m) },
                { 8.0m,  (7.5m, 41.5m, 26.0m) },
                { 9.0m,  (8.5m, 42.5m, 27.0m) },
                { 10.0m, (9.5m, 44.0m, 28.0m) },
                { 11.0m, (10.5m, 45.0m, 29.0m) },
                { 12.0m, (11.5m, 46.0m, 30.0m) }
            },

            [Audience.Female] = new()
            {
                { 5.0m,  (3.0m, 35.5m, 22.0m) },
                { 6.0m,  (4.0m, 36.5m, 23.0m) },
                { 7.0m,  (5.0m, 37.5m, 24.0m) },
                { 8.0m,  (6.0m, 38.5m, 25.0m) },
                { 9.0m,  (7.0m, 40.0m, 26.0m) },
                { 10.0m, (8.0m, 41.5m, 27.0m) }
            }
        };

        public decimal GetEquivalentSize(
            decimal size, 
            MeasureSizes measureType, 
            MeasureSizes targetMeasureType, 
            Audience audience)
        {
            var genderTable = _sizes[audience]
                .FirstOrDefault(row => 
                measureType switch
                {
                    MeasureSizes.US => row.Key == size,
                    MeasureSizes.UK => row.Value.UK == size,
                    MeasureSizes.EU => row.Value.EU == size,
                    MeasureSizes.CM => row.Value.CM == size,
                    _ => false
                });

            if (genderTable.Equals(default(KeyValuePair<decimal, (decimal UK, decimal EU, decimal CM)>)))
                throw new DomainException($"Size '{size}' not found for measure '{measureType}'");

            return targetMeasureType switch
            {
                MeasureSizes.US => genderTable.Key,
                MeasureSizes.UK => genderTable.Value.UK,
                MeasureSizes.EU => genderTable.Value.EU,
                MeasureSizes.CM => genderTable.Value.CM,
                _ => throw new DomainException($"Unsupported target measure type: {targetMeasureType}")
            };
        }
    }
}
