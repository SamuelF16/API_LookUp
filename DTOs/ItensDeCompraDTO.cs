using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_LookUp.DTOs
{
    public class ItensDeCompraDTO
    {
        public int IdItens { get; set; }
        public int IdProduto { get; set; }
        public int IdComprasUsuario { get; set; }
        public decimal PrecoUnitario { get; set; }
        public int QuantidadeProdutos { get; set; }
        // public decimal Subtotal => PrecoUnitario * QuantidadeProdutos;
        
        /* Produto */
        public string NomeProduto { get; set; } = null!;
        public string ImagemProduto { get; set; } = null!;
        public decimal PrecoProduto { get; set; }
        public int? OfertaDescontoProduto { get; set; }
        // public decimal PrecoComDescontoProduto { get; set; }
    }
}