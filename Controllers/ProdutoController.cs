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
    public class ProdutoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("ListarTodosProdutos")]
        public async Task<ActionResult<IEnumerable<Produtos>>> GetTodosProdutos()
        {
            var listaProduto = await _context.Produtos.ToListAsync();
            return Ok(listaProduto);
        }

        [HttpGet("ListarProdutoID")]
        public async Task<ActionResult<Produtos>> GetProdutosId(int id)
        {
            var Produto = await _context.Produtos.FindAsync(id);
            if (Produto == null)
            {
                return NotFound($"Não foi encontrado produto com o id {id}");
            }

            return Ok(Produto);
        }

        [HttpPost("AdicionarProduto")]
        public async Task<ActionResult<Produtos>> PostProdutos(Produtos Produto)
        {
            _context.Produtos.Add(Produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProdutosId), new { id = Produto.IdProduto }, Produto);
        }
    }
}