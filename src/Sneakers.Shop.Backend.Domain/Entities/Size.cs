using Sneakers.Shop.Backend.Domain.Abstractions;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Exceptions;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class Size : Entity
    {
        public MeasureSizes MeasurementType { get; private set; }
        public decimal Value { get; private set; }

        private Size() { }

        public Size(
            MeasureSizes measureSizes,
            decimal val) : base(Guid.NewGuid())
        {
            if (val <= 0)
                throw new DomainException($"Cannot add {val} less than or equal 0");

            MeasurementType = measureSizes;
            Value = val;
        }

    }
}
