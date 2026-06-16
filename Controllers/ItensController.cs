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
    public class ItensController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ItensController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("ListarTodosItens")]
        public async Task<ActionResult<IEnumerable<Itens>>> GetTodosItens()
        {
            var listaItens = await _context.Itens
                                .Include(i => i.Produto)
                                .Include(i => i.CompraUsuario)
                                .ToListAsync();

            if (listaItens == null || !listaItens.Any())
            {
                return NotFound("Não tem Itens no momento");
            }
            return Ok(listaItens);
        }

        [HttpGet("ListarItensID")]
        public async Task<ActionResult<Itens>> GetItensId(int id)
        {
            var Itens = await _context.Itens.FindAsync(id);
            if (Itens == null)
            {
                return NotFound($"Não foi encontrado item com o id {id}");
            }

            return Ok(Itens);
        }

        [HttpGet("ListarTodosItensDeUmaCompra")]
        public async Task<ActionResult<IEnumerable<ItensDeCompraDTO>>> GetItensDeUmaCompra(int idCompra)
        {
            var ItensDaCompra = await _context.Itens
                .Where(i => i.IdComprasUsuario == idCompra)
                .Select(c => new ItensDeCompraDTO
                {
                    IdItens = c.IdItens,
                    IdComprasUsuario = c.IdComprasUsuario,
                    PrecoUnitario = c.PrecoUnitario,
                    QuantidadeProdutos = c.QuantidadeProdutos,
                    //Subtotal = 
                    IdProduto = c.IdProduto, //Tenho que arrumar
                    NomeProduto = c.Produto.Nome,
                    ImagemProduto = c.Produto.Imagem,
                    PrecoProduto = c.Produto.Preco,
                    OfertaDescontoProduto = c.Produto.OfertaDesconto,
                    // PrecoComDescontoProduto =
                })
                .ToListAsync();

            if (ItensDaCompra == null || !ItensDaCompra.Any())
            {
                return NotFound($"Não foi possível encontrar nem iten na compra de id {idCompra}");
            }

            return Ok(ItensDaCompra);
        }
    }
}