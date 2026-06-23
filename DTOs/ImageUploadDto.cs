using System.ComponentModel.DataAnnotations;

namespace API_LookUp.DTOs
{
public class ImageUploadDto
{
    [Required]
    public IFormFile UserImage { get; set; } = null!;

    [Required]
    public IFormFile ProductImage { get; set; } = null!;
}
}