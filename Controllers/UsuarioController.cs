using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
// using System.Net.Http; // This line should remain uncommented if HttpClient is used elsewhere, but for GeminiService it's not directly needed here.
using API_LookUp.Models;
using API_LookUp.DTOs;
// using API_LookUp.Services;
using API_LookUp.Repository;
using Microsoft.EntityFrameworkCore;

namespace API_LookUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IGeminiService _geminiService;
        private readonly HttpClient _httpClient;

        public UsuarioController(AppDbContext context, IGeminiService geminiService, HttpClient httpClient)
        {
            _context = context;
            _geminiService = geminiService;
            _httpClient = httpClient;
        }

        [HttpGet("ListarTodosUsuarios")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetTodosUsuarios()
        {
            var listaUsuario = await _context.Usuario.ToListAsync();
            return Ok(listaUsuario);
        }

        [HttpGet("ListarUsuarioID")]
        public async Task<ActionResult<Usuario>> GetUsuariosId(int id)
        {
            var Usuario = await _context.Usuario.FindAsync(id);
            if (Usuario == null)
            {
                return NotFound($"Não foi encontrado usuario com o id {id}");
            }

            return Ok(Usuario);
        }

        [HttpGet("ComprasDoUsuario")]
        public async Task<ActionResult<IEnumerable<ComprasUsuario>>> GetComprasDoUsuario(int idUsuario)
        {
            var comprasDoUsuario = await _context.ComprasUsuario
                .Where(c => c.IdUsuario == idUsuario)
                .ToListAsync();

            if (comprasDoUsuario == null || !comprasDoUsuario.Any())
            {
                return NotFound($"Não foi possível encontrar nem uma compra para esse usuario de id {idUsuario}");
            }

            return Ok(comprasDoUsuario);
        }

        [HttpPost("LogarUsuario")]
        public async Task<ActionResult<Usuario>> LogarUsuario(string email, string senha)
        {
            var usuarioLogado = await _context.Usuario
                .FirstOrDefaultAsync(u => u.Email == email && u.SenhaCadastrada == senha);

            if (usuarioLogado == null)
            {
                return NotFound("Email ou senha incorretos.");
            }

            return Ok(usuarioLogado);
        }

        [HttpPost("RegistrarUsuario")]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            // Em uma aplicação real, você deve fazer o hash da senha antes de salvar.
            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuariosId), new { id = usuario.IdUsuario }, usuario);
        }

        [HttpPost("logoutUsuario")]
        public async Task<ActionResult> LogoutUsuario(int idUsuario)
        {
            var usuario = await _context.Usuario.FindAsync(idUsuario);
            if (usuario == null)
            {
                return NotFound($"Não foi encontrado usuario com o id {idUsuario}");
            }

            // Aqui você pode implementar a lógica de logout, como limpar tokens ou sessões, dependendo da sua implementação de autenticação.

            return Ok("Usuário deslogado com sucesso.");
        }

        [HttpPost("GerarPreviewComIA")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> GerarPreviewComIA(
            [FromForm] ImageUploadDto dto)
        {
            if (dto.UserImage is null || dto.ProductImage is null)
            {
                return BadRequest(new { error = "As imagens do usuário e do produto são obrigatórias." });
            }

            if (!IsAllowedImage(dto.UserImage) || !IsAllowedImage(dto.ProductImage))
            {
                return BadRequest(new { error = "Apenas imagens JPEG, PNG ou WEBP são permitidas." });
            }

            try
            {
                await using var userImageStream = dto.UserImage.OpenReadStream();
                await using var productImageStream = dto.ProductImage.OpenReadStream();

                var generatedBytes = await _geminiService.GeneratePreviewAsync(
                    userImageStream,
                    dto.UserImage.ContentType,
                    productImageStream,
                    dto.ProductImage.ContentType,
                    null);       // Prompt padrão

                return File(generatedBytes, "image/png", "previa_produto.png");
            }
            catch (Exception ex)
            {
                // Considere usar um logger (ILogger) para registrar a exceção
                return StatusCode(500, new { error = $"Erro ao gerar a pré-visualização: {ex.Message}" });
            }
        }

        private static bool IsAllowedImage(IFormFile file)
        {
            var allowed = new[] { "image/jpeg", "image/jpg", "image/png", "image/webp" };
            return allowed.Contains(file.ContentType.ToLowerInvariant());
        }
    }
}