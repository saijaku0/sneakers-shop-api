using Sneakers.Shop.Backend.Domain.Exceptions;

namespace Sneakers.Shop.Backend.Domain.ValueObjects
{
    public class SubmissionSize
    {
        public int Quantity { get; }
        public decimal SizeInCm { get; }

        public SubmissionSize(
            int quantity, 
            decimal sizeInCm)
        {
            if (quantity <= 0)
                throw new DomainException("Quantity must be greater than zero.", nameof(quantity));
            if (sizeInCm <= 0)
                throw new DomainException("Size value must be greater than zero.", nameof(sizeInCm));

            Quantity = quantity;
            SizeInCm = sizeInCm;
        }
    }
}
