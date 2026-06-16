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
    public class EnderecoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EnderecoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("ListarTodosEnderecosUsuarios")]
        public async Task<ActionResult<IEnumerable<EnderecoUsuario>>> GetTodosEnderecosUsuarios()
        {
            var listaEnderecoUsuario = await _context.EnderecoUsuario
                                            .Include(c => c.Usuario)
                                            .ToListAsync();

            if (listaEnderecoUsuario == null || !listaEnderecoUsuario.Any())
            {
                return NotFound("Não tem endereço no momento");
            }

            return Ok(listaEnderecoUsuario);
        }

        [HttpGet("ListarTodosEnderecosDeUmUsuario")]
        public async Task<ActionResult<IEnumerable<EnderecoComUsuarioDTO>>> GetEnderecosDeUmUsuario(int idUsuario)
        {
            var EnderecoDeUmUsuario = await _context.EnderecoUsuario
                .Where(i => i.IdUsuario == idUsuario)
                .Select(e => new EnderecoComUsuarioDTO
                {
                        IdEnderecoUsuario = e.IdEnderecoUsuario,
                        IdUsuario = e.IdUsuario,
                        Cep = e.Cep,
                        Estado = e.Estado,
                        Municipio = e.Municipio,
                        Bairro = e.Bairro,
                        Rua = e.Rua,
                        NumeroResidencial = e.NumeroResidencial,
                        Complemento = e.Complemento,
                        TipoEndereco = e.TipoEndereco,
                        NomeUsuario = e.Usuario.Nome,
                        EmailUsuario = e.Usuario.Email,
                        TelefoneUsuario = e.Usuario.Telefone,
                })
                .ToListAsync();

            if (EnderecoDeUmUsuario == null || !EnderecoDeUmUsuario.Any())
            {
                return NotFound($"Não foi possível encontrar nem um endereco para esse usuario de id {idUsuario}");
            }

            return Ok(EnderecoDeUmUsuario);
        }
    }
}