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
    public class CompraController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CompraController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("ListarTodasComprasUsuarios")]
        public async Task<ActionResult<IEnumerable<ComprasUsuario>>> GetTodasComprasUsuarios()
        {
            var listaComprasUsuario = await _context.ComprasUsuario
                                            .Include(c => c.Usuario)
                                            .ToListAsync();

            if (listaComprasUsuario == null || !listaComprasUsuario.Any())
            {
                return NotFound("Não tem compras no momento");
            }

            return Ok(listaComprasUsuario);
        }

        [HttpGet("ListarTodasComprasDoUsuario")]
        public async Task<ActionResult<IEnumerable<CompraComUsuarioDTO>>> GetComprasDeUmUsuario(int idUsuario)
        {
            var CompraDoUsuario = await _context.ComprasUsuario
                .Where(i => i.IdUsuario == idUsuario)
                .Select(c => new CompraComUsuarioDTO
                {
                    IdComprasUsuario = c.IdComprasUsuario,
                    IdUsuario = c.IdUsuario,
                    StatusCompra = c.StatusCompra.ToString(),
                    ValorTotal = c.ValorTotal,
                    DtaCompra = c.DtaCompra,
                    DtaFechamento = c.DtaFechamento,
                    NomeUsuario = c.Usuario.Nome,
                    EmailUsuario = c.Usuario.Email,
                    TelefoneUsuario = c.Usuario.Telefone
                })
                .ToListAsync();

            if (CompraDoUsuario == null || !CompraDoUsuario.Any())
            {
                return NotFound($"Não foi possível encontrar nem uma compra para esse usuario de id {idUsuario}");
            }

            return Ok(CompraDoUsuario);
        }

    }
}