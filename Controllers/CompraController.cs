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

        [HttpPost("AdicionarUmaCompraComItens")]
        public async Task<ActionResult<ComprasUsuario>> PostCompraComItens(CompraComItensDTO compraComItensDTO)
        {
            if (compraComItensDTO.Itens == null || !compraComItensDTO.Itens.Any())
                return BadRequest("A compra deve conter pelo menos um item");

            if (compraComItensDTO.ValorTotal <= 0)
                return BadRequest("Valor total deve ser maior que zero");

            try
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    var compraNova = new ComprasUsuario
                    {
                        IdUsuario = compraComItensDTO.IdUsuario,
                        StatusCompra = compraComItensDTO.StatusCompra,
                        ValorTotal = compraComItensDTO.Itens
                                    .Sum(i => i.PrecoUnitario * i.QuantidadeProdutos),
                        DtaCompra = compraComItensDTO.DtaCompra
                    };

                    _context.ComprasUsuario.Add(compraNova);
                    await _context.SaveChangesAsync();

                    foreach (var item in compraComItensDTO.Itens)
                    {
                        var itemNovo = new Itens
                        {
                            IdComprasUsuario = compraNova.IdComprasUsuario,
                            IdProduto = item.IdProduto,
                            PrecoUnitario = item.PrecoUnitario,
                            QuantidadeProdutos = item.QuantidadeProdutos
                        };
                        _context.Itens.Add(itemNovo);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Ok(compraNova);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar compra: {ex.Message}");
            }
        }

        [HttpPut("AtualizarStatusCompra")]
        public async Task<ActionResult> PutAtualizarStatusCompra(int idCompra, ComprasStatus novoStatus)
        {
            var compra = await _context.ComprasUsuario.FindAsync(idCompra);
            if (compra == null)
            {
                return NotFound($"Compra com id {idCompra} não encontrada");
            }

            compra.StatusCompra = novoStatus;
            _context.Entry(compra).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ComprasUsuario.Any(e => e.IdComprasUsuario == idCompra))
                {
                    return NotFound($"Compra com id {idCompra} não encontrada");
                }
                else
                {
                    throw;
                }
            }
        }
    }
}