namespace Sneakers.Shop.Backend.Api.Validators
{
    /// <summary>
    /// Provides methods for validating uploaded files against predefined constraints such as allowed file types,
    /// maximum file size, and file count.
    /// </summary>
    /// <remarks>This class is intended for use in scenarios where user-uploaded files must be validated
    /// before processing or storage. It enforces restrictions on file extensions, individual file size, and the total
    /// number of files allowed in a single upload operation. All members are static and thread safe.</remarks>
    public static class FileValidator
    {
        private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png"];
        private const int MaxFileSize = 5 * 1024 * 1024;
        private const int MaxFilesCount = 7;

        /// <summary>
        /// Validates a collection of uploaded files against allowed file count, size, and format constraints.
        /// </summary>
        /// <remarks>Allowed file formats are .jpg and .png. Each file must not exceed the maximum allowed
        /// size of 5MB. The method returns after the first validation failure encountered.</remarks>
        /// <param name="files">The collection of files to validate. Must contain at least one file and no more than the maximum allowed
        /// number of files.</param>
        /// <returns>A tuple containing a value indicating whether the files are valid and an error message if validation fails.
        /// If validation succeeds, the error message is null.</returns>
        public static (bool IsValid, string? Error) Validate(IFormFileCollection files)
        {
            if (files.Count == 0)
                return (false, "At least one file is required.");
            if (files.Count > MaxFilesCount)
                return (false, $"Maximum {MaxFilesCount} files allowed.");

            foreach (var file in files)
            {
                if (file.Length > MaxFileSize)
                    return (false, $"File '{file.FileName}' exceeds maximum size of 5MB.");

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!AllowedExtensions.Contains(extension))
                    return (false, $"File '{file.FileName}' has unsupported format. Allowed: jpg, png.");
            }

            return (true, null);
        }
    }
}
