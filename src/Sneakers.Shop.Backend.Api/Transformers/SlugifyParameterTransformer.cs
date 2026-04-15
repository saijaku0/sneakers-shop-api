using System.Text.RegularExpressions;

namespace Sneakers.Shop.Backend.Api.Transformers
{
    /// <summary>
    /// Transforms parameter values into a "slug" format for use in URL routes.
    /// </summary>
    /// <remarks>
    /// This transformer converts parameter values by separating words with hyphens and converting them
    /// to lowercase. For example, "MyValue" will be transformed into "my-value". It is typically used
    /// to generate human-readable URLs in ASP.NET Core routing.
    /// </remarks>
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        /// <summary>
        /// Converts the specified object's string representation from PascalCase or camelCase to a lowercase,
        /// hyphen-separated format suitable for outbound use.
        /// </summary>
        /// <remarks>This method inserts hyphens between lowercase and uppercase letter boundaries and
        /// converts the result to lowercase. It is useful for transforming .NET property names to formats commonly used
        /// in URLs or JSON keys.</remarks>
        /// <param name="value">The object to convert. If null, the method returns null. The object's ToString() method is used to obtain
        /// its string representation.</param>
        /// <returns>A lowercase, hyphen-separated string representation of the input value, or null if the input is null.</returns>
        public string? TransformOutbound(object? value)
        {
            if (value == null) return null;
            return Regex.Replace(
                value.ToString()!,
                "([a-z])([A-Z])",
                "$1-$2",
                RegexOptions.CultureInvariant,
                TimeSpan.FromMilliseconds(100)
            ).ToLowerInvariant();
        }
    }
}
