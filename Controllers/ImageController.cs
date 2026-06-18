using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API_LookUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IGeminiService _geminiService;

        public ImageController(IGeminiService geminiService)
        {
            _geminiService = geminiService;
        }

        [HttpPost("gerar-imagem")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Generate(
            [FromForm] IFormFile pessoaImage,
            [FromForm] IFormFile produtoImage,
            [FromForm] string? prompt = null)
        {
            if (pessoaImage is null || produtoImage is null)
            {
                return BadRequest(new { error = "Envie as duas imagens: pessoaImage e produtoImage." });
            }

            if (!IsAllowedImage(pessoaImage) || !IsAllowedImage(produtoImage))
            {
                return BadRequest(new { error = "Apenas imagens JPEG, PNG ou WEBP são permitidas." });
            }

            try
            {
                await using var pessoaStream = pessoaImage.OpenReadStream();
                await using var produtoStream = produtoImage.OpenReadStream();

                var imageBytes = await _geminiService.GeneratePreviewAsync(
                    pessoaStream,
                    pessoaImage.ContentType,
                    produtoStream,
                    produtoImage.ContentType,
                    prompt);

                return File(imageBytes, "image/png", "preview.png");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        private static bool IsAllowedImage(IFormFile file)
        {
            var allowed = new[] { "image/jpeg", "image/png", "image/webp" };
            return allowed.Contains(file.ContentType);
        }
    }
}