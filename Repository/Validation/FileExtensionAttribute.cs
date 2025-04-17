using System.ComponentModel.DataAnnotations;

namespace WebApp.Repository.Validation
{
	public class FileExtensionAttribute : ValidationAttribute
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			if(value is IFormFile file)
			{
				var extension = Path.GetExtension(file.FileName); //123.jpg
				string[] extensions = { "jpg", "png", "jpeg", "jfif", "webp" };

				bool result = extensions.Any(x => extension.EndsWith(x));

				if(!result)
				{
					return new ValidationResult("Allowed extensions of image");
				}
			}
			return ValidationResult.Success;
		}
	}
}
