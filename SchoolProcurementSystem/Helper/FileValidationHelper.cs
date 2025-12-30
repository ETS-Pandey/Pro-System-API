namespace SchoolProcurement.Api.Helper
{
    public static class FileValidationHelper
    {
        // Allowed file extensions (lowercase)
        private static readonly HashSet<string> AllowedExtensions = new HashSet<string>
        {
            ".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx", ".xls", ".xlsx"
        };

        // Allowed MIME types as extra safety
        private static readonly HashSet<string> AllowedMimeTypes = new HashSet<string>
        {
            "application/pdf",
            "image/jpeg",
            "image/png",
            "application/msword",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "application/vnd.ms-excel",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        };

        // Max 10 MB file size
        private const long MaxFileBytes = 10 * 1024 * 1024; // 10 MB

        /// <summary>
        /// Validates a single uploaded file
        /// </summary>
        public static FileValidationResult ValidateFile(IFormFile file)
        {
            if (file == null)
                return FileValidationResult.Fail("File is empty.");

            if (file.Length == 0)
                return FileValidationResult.Fail("File is empty.");

            if (file.Length > MaxFileBytes)
                return FileValidationResult.Fail($"File is too large. Max allowed size is {MaxFileBytes / (1024 * 1024)} MB.");

            var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(ext) || !AllowedExtensions.Contains(ext))
                return FileValidationResult.Fail($"File extension '{ext}' is not allowed.");

            // MIME validation (not 100% reliable but extra safety)
            if (!string.IsNullOrWhiteSpace(file.ContentType))
            {
                var contentType = file.ContentType.ToLowerInvariant();
                if (!AllowedMimeTypes.Contains(contentType))
                    return FileValidationResult.Fail($"MIME type '{file.ContentType}' is not allowed.");
            }

            return FileValidationResult.Success();
        }
    }

    public class FileValidationResult
    {
        public bool IsValid { get; }
        public string? ErrorMessage { get; }

        private FileValidationResult(bool isValid, string? errorMessage)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }

        public static FileValidationResult Success()
            => new FileValidationResult(true, null);

        public static FileValidationResult Fail(string message)
            => new FileValidationResult(false, message);
    }
}
