using Sneakers.Shop.Backend.Application.Submissions.DTOs;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Interfaces;

namespace Sneakers.Shop.Backend.Application.Submissions.Helpers
{
    public static class SubmissionSizeMapper
    {
        public static IEnumerable<(int Quantity, decimal SizeInCm)> MapSizes(
            IEnumerable<SubmissionSizeDto> sizes,
            Audience audience,
            ISizeConversionService sizeConversion)
        {
            return sizes.Select(s =>
            {
                var sizeInCm = sizeConversion.GetEquivalentSize(
                    s.SizeValue,
                    s.MeasureType,
                    MeasureSizes.CM,
                    audience
                );
                return (s.Quantity, sizeInCm);
            });
        }
    }
}
