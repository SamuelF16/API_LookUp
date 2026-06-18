using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API_LookUp.Models;
using API_LookUp.DTOs;
using API_LookUp.Repository;
using Microsoft.EntityFrameworkCore;

namespace API_LookUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
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
            var usuarioNovo = new Usuario
            {
                IdUsuario = usuario.IdUsuario,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                SenhaCadastrada = usuario.SenhaCadastrada
            };
            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(usuarioNovo);
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

        [HttpPost("SalvarFotoTesteDeIA")]
        public async Task<ActionResult> SalvarFotoTesteDeIA(int idUsuario, string urlFoto)
        {
            var usuario = await _context.Usuario.FindAsync(idUsuario);
            if (usuario == null)
            {
                return NotFound($"Não foi encontrado usuario com o id {idUsuario}");
            }

            // Aqui você pode implementar a lógica para salvar a URL da foto no banco de dados ou em um serviço de armazenamento, dependendo da sua arquitetura.

            return Ok("Foto salva com sucesso.");
        }
    }
}