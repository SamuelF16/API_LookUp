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
            var produtoNovo =new Produtos
            {
                IdProduto = Produto.IdProduto,
                Nome = Produto.Nome,
                Modelo = Produto.Modelo,
                Imagem = Produto.Imagem,
                Preco = Produto.Preco,
                OfertaDisponivel = Produto.OfertaDisponivel,
                QuantidadeEstoque = Produto.QuantidadeEstoque
            };
            _context.Produtos.Add(Produto);
            await _context.SaveChangesAsync();

            return Ok(produtoNovo);
        }

        [HttpGet("BuscarProdutosNome")]
        public async Task<ActionResult<IEnumerable<Produtos>>> GetBuscarProdutosNome([FromQuery] string search = null)
        {
            var query = _context.Produtos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var termo = search.Trim();

                if (Enum.TryParse(typeof(ModeloProduto), termo, true, out var modeloEnum))
                {
                    query = query.Where(p =>
                        p.Nome.ToLower().Contains(termo.ToLower()) ||
                        p.Descricao.ToLower().Contains(termo.ToLower()) ||
                        p.Modelo.Equals(modeloEnum)
                    );
                }
                else
                {
                    query = query.Where(p =>
                        p.Nome.ToLower().Contains(termo.ToLower()) ||
                        p.Descricao.ToLower().Contains(termo.ToLower())
                    );
                }
            }

            var lista = await query.ToListAsync();
            return Ok(lista);
        }
    }
}