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
    }
}